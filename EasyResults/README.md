# EasyResults

EasyResults is a C# library that makes it easy to handle results according to the status code received. It allows developers to define how to handle success, client error and server errors. It also allows developers to handle results according to the specific status code returned. 

## Installation: 

The EasyResults library is available as a NuGet package. To install it, open the Package Manager Console in Visual Studio and run the following command: ```Install-Package EasyResults```.

## How to use

### Classes overview

```Result<T>``` is the class that represents the output that will be handled, where T represents the type of data you are handling, for example, if you are getting an user by id you will have a return of ```Result<User>```. In the result you can also define a Status and a Message.

```ResultHandler<T, T2>``` class takes two generic types, where T is the same type of ```Result<T>``` and T2 is the type of the return of the handling, for example, if you are getting an user by id and you want to use EasyResults in the API Controller, you might have ```ResultHandler<User, ActionResult>```.

### Available status

You have a total of 7 defined status available which split into 3 groups

#### Success

- Status.Success = 1

#### Client error

- Status.BadRequest = 2
- Status.Unauthorized = 3
- Status.Forbidden = 4
- Status.NotFound = 5
- Status.Conflict = 6

#### Server error

- Status.InternalServerError = 7

#### Custom status

You can also pass a custom status to ```Result<T>```. Custom status must have values greater than 1000 since values between 8 and 999 are reserved.

### Handling results

There are two ways you can use library to handle results.

#### Via Action and Execute methods

The first way you need to define the Action that will return the result to handle. The return type must be ```Result<T>```.

```csharp
return new ResultHandler<User, bool>()
	.Action(() =>
	{
		return this.UsersService.CreateUser(user);
	})
	.OnSuccess(serviceResult => true)
	.OnClientError(serviceResult => false)
	.OnServerError(serviceResult => false)
	.OnCustomStatus(serviceResult => false)
	.Execute();
```

On the code above we create an instance of new ```ResultHandler<User, bool>```. It defines the ```Action``` method  which takes a lambda expression that represents the service operation to be executed and returns a ```Result<User>```. 

Afterward it sets the behaviour handlers ```OnSuccess```, ```OnClientError```, ```OnServerError``` and ```OnCustomStatus``` methods, which define the desired result for a successful operation, client-side error, server-side error, and custom status handling respectively.

Finally, it executes the service operation by calling ```Execute``` method and returns the appropriate result based on the outcome of the operation and the provided lambda expressions.

#### Via HandleResult method

The second way you don't define the ```Action``` method nor the ```Execute``` method. Instead, it is expected you already have the ```Result<T> ```object and you pass it to the ```HandleResult``` method which will return the result based on the result provided and the provided lambda expressions.

```csharp
Result<User> result = this.UsersService.CreateUser(user);

return new ResultHandler<User, bool>()
	.OnSuccess(serviceResult => true)
	.OnClientError(serviceResult => false)
	.OnServerError(serviceResult => false)
	.OnCustomStatus(serviceResult => false)
	.HandleResult(user);
```

#### Other features

##### OnStatus method

The method ```OnStatus``` allows to define the result behaviour for a specific status, being more specific that other behaviour handlers.

```csharp
Result<User> result = this.UsersService.CreateUser(user);

return new ResultHandler<User, string>()
	.OnSuccess(serviceResult => "true")
	.OnClientError(serviceResult => "false")
	.OnServerError(serviceResult => "false")
	.OnCustomStatus(serviceResult => false)
	.OnStatus(Status.BadRequest, _ => "BadRequest")
	.HandleResult(user);
```

On the above code, ```ResulHandler``` returns a string. 

If the ```CreateUser``` method returns success then ```ResulHandler``` returns "true".

If the ```CreateUser``` method returns any error different from BadRequest then ```ResultHandler``` returns "false".

If the ```CreateUser``` method returns BadRequest then ```ResultHandler``` returns "BadRequest".

##### OnUnhittedHandler method

The method ```OnUnhittedHandler``` allows to define the result behaviour in case of no other handler was hitted.

```csharp
Result<User> result = this.UsersService.CreateUser(user);

return new ResultHandler<User, string>()
	.OnSuccess(serviceResult => "true")
	.OnUnhittedHandler(serviceResult => "UnhittedHandler")
	.HandleResult(user);
```

On the above code, ```ResulHandler``` returns a string. 

If the ```CreateUser``` method returns success then ```ResulHandler``` returns "true".

If the ```CreateUser``` method returns an error then ```ResultHandler``` returns "UnhittedHandler".

##### Defining behavious

On  the ```OnSuccess```, ```OnClientError```, ```OnServerError```, ```OnCustomStatus```, ```OnStatus``` and ```OnUnhittedHandler``` methods you can access the ```Result<T>``` object that it is being handled on the lambda input.

```csharp
Result<User> result = this.UsersService.CreateUser(user);

return new ResultHandler<User, bool>()
	.OnSuccess(_ => true)
	.OnClientError(_ => true)
	.OnServerError(serviceResult => {
		Log.Critical(serviceResult.Message);
		return false; 
	})
	.OnCustomStatus(serviceResult => {
		return (int)serviceResult.Status == 1000
	});
```