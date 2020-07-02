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


## Update 2020-07-01
- I still believe razor syntax is clunky and too verboose.
- Cascading Values and Cascading parameters are cool though they are kind of magic and spooky? I'm not convinced yet
- Routing is simple and easy I like it
- I don't like the idea of passing Callbacks Up as parameters (props) in components I still prefer the CustomEvent + bubbling of standard javascript
- Form validation is nice and easy until it isn't

In regards to the backend, I think C# is a solid option if you like the language I enjoy the backend side of this project more than the Frontend in C#

> - I still believe razor syntax is clunky and too verboose.

Indeed, check the [RecipeForm](https://github.com/AngelMunoz/Recetron/blob/master/Recetron/Shared/RecipeForm.razor) file 10 lines in the file and you're already at an `@if`
```html
<article class="rec-recipe-form recbg-light">
  <nav class="navbar" if.bind="isEditing && recipe">
    <section class="navbar-section">
      <div class="navbar-item">
        <label class="form-checkbox form-inline">
          <input type="checkbox" value.two-way="ignorePending" />
          <i class="form-icon"></i> Ignore pending changes
        </label>
      </div>
    </section>
    <section class="navbar-section">
      <button class="btn btn-primary" click.delegate="TriggerForm()">
        Save Recipe
      </button>
    </section>
  </nav>
  <div class="edit-section" if.bind="isEditing && recipe">
    <div class="edit-section">
      <section class="form-section recbg-info">
        <header class="section-header recbg-light">
          Recipe Ingredients
        </header>
        <ul class="ingredient-list">
          <li repeat.for="ingredient of recipe.Ingredients" class="ingredient-list-item">
              <input class="recbg-default ingredient-input name" type="text" value.bind="ingredient!.Name" />
              <input class="recbg-default ingredient-input" type="text" value.bind="ingredient!.Amount" />
              <input class="recbg-default ingredient-input" type="text" value.bind="ingredient!.Unit" />
              <span class="c-hand" click.delegate="DeleteIngredient(ingredient)"><Trash BackgroundColor="#fff" /></span>
          </li>
        </ul>
      </section>
    </div>
  </div>
  <div if.bind="!isEditing && recipe" class="view-section">
    <section class="recipe-section">
    </section>
  </div>
</article>
```
vs
```html
<article class="rec-recipe-form recbg-light">
  @if (isEditing && recipe != null) {
    <nav class="navbar">
      <section class="navbar-section">
        <div class="navbar-item">
          <label class="form-checkbox form-inline">
            <input type="checkbox" @bind-Value="ignorePending" />
            <i class="form-icon"></i> Ignore pending changes
          </label>
        </div>
      </section>
      <section class="navbar-section">
        <button class="btn btn-primary" @onclick="@(_ => TriggerForm())">
          Save Recipe
        </button>
      </section>
    </nav>
    <div class="edit-section">
      <section class="form-section recbg-info">
        <header class="section-header recbg-light">
          Recipe Ingredients
        </header>
        <ul class="ingredient-list">
          @foreach (var ingredient in recipe.Ingredients)
          {
            <li @key="ingredient!.Name" class="ingredient-list-item">
              <input class="recbg-default ingredient-input name" type="text" @bind-value="ingredient!.Name" />
              <input class="recbg-default ingredient-input" type="text" @bind-value="ingredient!.Amount" />
              <input class="recbg-default ingredient-input" type="text" @bind-value="ingredient!.Unit" />
              <span class="c-hand" @onclick="@(_ => DeleteIngredient(ingredient))"><Trash BackgroundColor="#fff" /></span>
            </li>
          }
        </ul>
      </section>
    </div>
  }
  else
  {
    <div class="view-section">
      <section class="recipe-section">
      </section>
    </div>
  }
</article>
```
I don't know about you but I like my HTML without braces cluttering my view that example above is a look-alike with [Aurelia](https://aurelia.io/) now you could argue that every part of that form can be split into smaller components and I'd agree with that right now I'm just learning blazor and perhaps there are better ways to structure your controls but if I consider myself to be a very average programmer chances are there will be code like this in a few years in some pproduction sites

> - Cascading Values and Cascading parameters are cool though they are kind of magic and spooky? I'm not convinced yet

I'm still grasping this concept I guess this is like [React's Context](https://reactjs.org/docs/context.html) you put something on the root or some levels up in the hierarchy and you are able to consume those values down the hierarchy seems neat, but not sure if that's something I'd like to rely on

> - Routing is simple and easy I like it

Add a razor component and `@route "/my/route"` at the top and it just works, nice indeed

> - I don't like the idea of passing Callbacks Up as parameters (props) in components I still prefer the CustomEvent + bubbling of standard javascript

the thing about callbacks and what we used to do in jsland is that they get nested and nested and nested to the point you have a pyramid of HTML trying to do something with callbacks I think react folks used to have this issue (not sure if they have solved it today)

this is where I like to rely on the standard dom event bubbling, it doesn't matter where below the hierarchy you dispatched an event, as long as it bubbles you can grab it with a listener, for example in aurelia
```html
<!-- MyViewModel.html -->
<div my-event.delegate="doSometing($event)">
  <div>
    <div>
    <!-- catchit here -->
      <div>
      <!-- catch it here -->
        <div>
          <my-child-component></my-child-component>
        </div>
      </div>
    </div>
  </div>
</div>
```
```js
class MyViewModel {

  doSometing(event) { console.log(event.detail) ;}
}
```
if `my-child-component` fires `my-event` you can listen for it on every element upwards in that example I'm just puting some divs but you can have a complex hierarchy with different custom components that can act on that delegate and that's very powerful on my opinion.

> - Form validation is nice and easy until it isn't

If my forms are a single top level data type, then I don't have any issues as seen in the [Auth](https://github.com/AngelMunoz/Recetron/blob/master/Recetron/Pages/Auth.razor) component

```C#
public class MyType 
{
  [Required]
  public string Name {get; set;}
}
```
but once you have a complext type (as in [RecipeForm](https://github.com/AngelMunoz/Recetron/blob/master/Recetron/Shared/RecipeForm.razor))... then it gets complicated because right now there are no out of the box soulutions for that, there's a preview package
```c#
public class MyType
{
  [Required, MaxLength(30)]
  public string Name {get; set;}

  [Required]
  public SecondType MySecondType {get; set;}
}
```
that allows you to analyze a complex type graph as long as it has data annotations but it's not released yet I guess for the moment I'd drop EditForms completely and rely on `FluentValidator` since I'm already using that to validate the backend models before processing them on their respective endpoints I'd argue that you should be able to share the Abstract Validators between client/server


### Updated opinion so far?

Blazor it's somewhat a csharpilized react? it feels react'ish on some parts and even angular'ish on others but this is for sure made for C# devs as a primarily Javascript dev I feel like I'm writing a lot of code for moreless the same result if I add typescript I can start getting typesafety without losing the vast javascript ecosystem where a lot of the problems of the web are likely to be solved and not "yet to be discover"

- I like it but I still need to do more work with it to see if I could actually swap it for Javascript
