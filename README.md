# Recetron

[Blazor]: https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor
[Carter]: https://github.com/CarterCommunity/Carter
[MongoDB]: https://mongodb.github.io/mongo-csharp-driver/
[MongoDB Atlas]: https://www.mongodb.com/cloud/atlas
[Bolero]: https://fsbolero.io/
[Heroku]: https://elements.heroku.com/buildpacks/jincod/dotnetcore-buildpack
[Firebase]: https://firebase.google.com/docs/hosting/

This is a simple playground with a "full-stack" C# web application using [Blazor], [Carter] (asp.net) and [MongoDB] (because I might try to migrate from a JS job to a .net job and blazor seems an easy path for that)

If I ever finish this project (will update this line) it will be somewhat a recipe keeper I plan to host it on [Firebase], [Heroku] and [MongoDB Atlas]


## Development
Just clone and add your `appsettings.Development.json` to your `wwwroot` directory on the web project to add your local values like your local API (if you're working with other API)
like
```json
{
  "apiUrl": "http://localhost:5000"
}
```


## Run
open side by side terminals (either using visual studio/vscode) and run 
- `dotnet watch -p [PROJECT NAME]`

example:
- T1: `dotnet watch -p Recetron.Api run`
- T2: `dotnet watch -p Recetron run`

To debug the API you can either attach the vscode debugger or just use Visual Studio to launch as debug
to debug the Web app... well let's say Console might be your best friend for now




### Thoughts so far...

> Please take in mind that my background is primarily Javascript so I'm biased towards javascript and I still prefer Javascript for the frontend, backend wise C# seems like a good fit

Well... I still like better Javascript for web applications, there's just so much ecosystem/diversity and the fact that js is dynamic makes some things just easier,like re-constructing objects, copy objects, manipulate JSON, better eventing in place (events and custom events)

You don't always need IoC and DI and blazor kind of forces you to use DI and create services to compensate the lack of better browser event integration.
The razor syntax is clunky I'd rather have less disrupting markup and code like Aurelia's `if.bind="exp"` or `repeat.for="item of items"` instead using tons of lines for ifs and control structures.

Form validation is quite nice actually I like that a lot it simplifies a lot of things I believe by just adding annotations

Also not having direct contact with JS comes with other benefits, it forces you to have less dependencies for minimal things like a CSS framework with it's own JS included, or a library for datetime conversions or string formatting which may not seem much but once you realize you have dependencies you might have too much of them already

Beyond that, I think Blazor might be a good thing though I'd favor the [Bolero] flavor

Carter on the backend, seems to simplify a lot of the boilerplate and code that comes from default asp.net controllers and supports open API (though I won't use it on this project) I believe it has a lighter feeling similar to [Express] which is a good thing in my opinion, I don't like to "over-pattern" stuff just for the sake of "Big corp says I must do it that way so I will do it that way" as long as the code is maintainable I will like it and if anything goes awry this is ASP under the hood so I can fallback to "Big Corp Patterns"