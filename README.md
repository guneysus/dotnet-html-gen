# csharp-html-gen

```csharp
var doc = document(
    doctype(

    ),

    head(
        title()
    ),
    body (
      h1(

      ),
      p(

      ),
      a().href().text(),
      
      a(href="", text=""),
      
      a(
          img(

          )
      )
    )
);

public class HtmlDocument {

}

public static class html {
    public static HtmlDocument document();
}
```
