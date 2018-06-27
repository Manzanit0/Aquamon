## Aquamon
A minimalistic cli for accessing Salesforce's Tooling API

### Features

- Creation and refresh of sandboxes.
- Progress check of sandbox refresh status.
- Configuration via CLI

### Configuration

Before using the application you must add your Production credentials to the `appsettings.json` as well as any other
parameters you want to use for every command, like the Post-Sandbox-Copy script Id.

Your appsettings.json should look something like this:

```
{
    "SalesforceConfiguration": {
        "LoginInfo": {
            "Username": "myusername@domain.org",
            "Password": "Sal3sf0rc3123",
            "SecurityToken": "kqkOzDLSElGGisuRF9S4qzGu",
            "ClientId": "3MVG98_Psg5cppybek7esekQkzql9LLOkBAO4MrEYIOGDDpN2VR9CkrPl1BBxh1cH4vrFrtv68MRNE9I3zGFw",
            "ClientSecret": "2492353313781879034"
        },
        "SandboxInfo": {
            "AutoActivate": true,
            "ApexClassId": "01p58000007eUTf"
        }
    }
}
```

You can grab the `ApexClassId` from the Salesforce URL: 
`https://**your instance**.lightning.force.com/lightning/setup/ApexClasses/page?address=%2F01p58000007eUTf`, although
don't take the the encoded '/' character.

The `ClientId` and `ClientSecret` you will obtain from the Connected Application you have created in the `Setup > App Manager`

### Usage

For convenience, there is a configuration command within Aquamon which allows you to change the values without having to 
search for the `appsettings.json` all the time. The config command will work as a getter or a setter depending on the 
amount of arguments you put into it:

The following will give you your current user name:

```
aquamon config LoginInfo.Username
```

While the next will change the username to `another.username@mydomain.org`:

```
aquamon config LoginInfo.Username "another.username@mydomain.org"
```

Once you have set the application's configuration, we can start automating.

In order to create a new sandbox, simply execute:

```
aquamon create "MySandbox" -d "Some description"
```

In case you want to override the ApexClassId set in the configuration, you can do so by adding the option `-a`:

```
aquamon create "MySandbox" -d "Some description" -a "213xDfdx2315464"
```

Refreshing sandboxes works exactly the same way, just swapping the command:

```
aquamon refresh "MySandbox" -d "Some description" -a "213xDfdx2315464"
```

Lastly, there is also a command to check the status of a sandbox:

```
aquamon status "MySandbox" "InProgress"
```

Currently Aquamon searches for a certain sandbox in a certain status, but hopefully down the road we can improve it.