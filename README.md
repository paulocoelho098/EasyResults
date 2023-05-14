# EasyResults

EasyResults is a C# library that makes it easy to handle results according to the status code received. It allows developers to define how to handle success, client error and server errors. It also allows developers to handle results according to the specific status code returned. 

## Installation: 

The EasyResults library is available as a NuGet package. To install it, open the Package Manager Console in Visual Studio and run the following command: ```Install-Package EasyResults```.

## How to use

### Interface and classes overview

```IResult``` is an interface that represents the output to be handled

```Result``` is a class that implements ```IResult``` and represents the output that can be handled by the application

```Result<T>``` is a class that implements ```IResult``` and represents the output that can be handled, where T represents the type of data you are handling, for example, if you are getting an user by id you will have a return of ```Result<User>```. In the result you can also define a Status and a Message.

```ResultHandler<T>``` class takes a generic type, where T is the type of the return of the handling, for example, if you are getting an user by id and you want to use EasyResults in the API Controller, you might have ```ResultHandler<ActionResult>```.

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

You can also pass a custom status to ```IResult``` implementations. Custom status must have values greater than 1000 since values between 8 and 999 are reserved.

### Handling results

There are two ways you can use library to handle results.

#### Via Action and Execute methods

The first way you need to define the Action that will return the result to handle. The return type must be ```IResult```.

```csharp
return new ResultHandler<bool>()
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

On the code above we create an instance of new ```ResultHandler<bool>```. It defines the ```Action``` method  which takes a lambda expression that represents the service operation to be executed and returns a ```IResult``` that can be a ```Result<User>```. 

Afterward it sets the behaviour handlers ```OnSuccess```, ```OnClientError```, ```OnServerError``` and ```OnCustomStatus``` methods, which define the desired result for a successful operation, client-side error, server-side error, and custom status handling respectively.

Finally, it executes the service operation by calling ```Execute``` method and returns the appropriate result based on the outcome of the operation and the provided lambda expressions.

#### Via HandleResult method

The second way you don't define the ```Action``` method nor the ```Execute``` method. Instead, it is expected you already have the ```IResult```object and you pass it to the ```HandleResult``` method which will return the result based on the result provided and the provided lambda expressions.

```csharp
IResult result = this.UsersService.CreateUser(user);

return new ResultHandler<bool>()
	.OnSuccess(serviceResult => true)
	.OnClientError(serviceResult => false)
	.OnServerError(serviceResult => false)
	.OnCustomStatus(serviceResult => false)
	.HandleResult(result);
```

#### Other features

##### OnStatus method

The method ```OnStatus``` allows to define the result behaviour for a specific status, being more specific that other behaviour handlers.

```csharp
Result<User> result = this.UsersService.CreateUser(user);

return new ResultHandler<string>()
	.OnSuccess(serviceResult => "true")
	.OnClientError(serviceResult => "false")
	.OnServerError(serviceResult => "false")
	.OnCustomStatus(serviceResult => false)
	.OnStatus(Status.BadRequest, _ => "BadRequest")
	.HandleResult(result);
```

On the above code, ```ResulHandler``` returns a string. 

If the ```CreateUser``` method returns success then ```ResulHandler``` returns "true".

If the ```CreateUser``` method returns any error different from BadRequest then ```ResultHandler``` returns "false".

If the ```CreateUser``` method returns BadRequest then ```ResultHandler``` returns "BadRequest".

##### OnUnhittedHandler method

The method ```OnUnhittedHandler``` allows to define the result behaviour in case of no other handler was hitted.

```csharp
Result<User> result = this.UsersService.CreateUser(user);

return new ResultHandler<string>()
	.OnSuccess(serviceResult => "true")
	.OnUnhittedHandler(serviceResult => "UnhittedHandler")
	.HandleResult(result);
```

On the above code, ```ResulHandler``` returns a string. 

If the ```CreateUser``` method returns success then ```ResulHandler``` returns "true".

If the ```CreateUser``` method returns an error then ```ResultHandler``` returns "UnhittedHandler".

##### Defining behavious

On  the ```OnSuccess```, ```OnClientError```, ```OnServerError```, ```OnCustomStatus```, ```OnStatus``` and ```OnUnhittedHandler``` methods you can access the ```IResult``` object that it is being handled on the lambda input.

```csharp
Result<User> result = this.UsersService.CreateUser(user);

return new ResultHandler<bool>()
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

If the ```IResult``` is of type ```Result<T>``` and you want to access Data inside the lambda you need to convert the ```IResult``` to ```Result<T>``` like shown below

```csharp
.OnCustomStatus(serviceResult => {
	return ((Result<User>)serviceResult).Data!.Id == 0
});
```