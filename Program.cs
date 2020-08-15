using System;
using System.Collections.Generic;
using System.Linq;
using static html;
using static System.Console;

namespace html_gen
{
    class Program
    {
        static void Main(string[] args)
        {
            Func<HtmlDocument> doc = default;
            // doc = document();
            doc = document(
                head(
                    title("Lorem ipsum di amet.")
                ),
                tag("body", tag("div", tag("p", text("hello world..."))))
            );

            WriteLine(doc().html());
        }
    }
}

public abstract class HtmlElement
{

    public virtual string html()
    {
        if (SelfClosing) return $"<{Tag}/>";

        return $"<{Tag}>{string.Join(Environment.NewLine, Children.Select(x => x().html()))}</{Tag}>";
    }

    public abstract string Tag { get; }
    public virtual bool SelfClosing { get; } = false;

    protected List<Func<HtmlElement>> Children { get; } = new List<Func<HtmlElement>>();

}

public class HtmlDocument : HtmlElement
{
    private const string DEFAULT_DOC_TYPE = "html";

    public string DocType { get; protected set; } = DEFAULT_DOC_TYPE;

    public override string Tag => throw new NotImplementedException();

    public override bool SelfClosing => throw new NotImplementedException();

    public override string html()
    {
        return $"<!DOCTYPE {DocType}>{string.Join(Environment.NewLine, Children.Select(x => x().html()))}";
    }

    public HtmlDocument() : this(DEFAULT_DOC_TYPE) { }
    public HtmlDocument(string docType) => DocType = docType;

    public HtmlDocument(Func<HtmlElement> child) : this(DEFAULT_DOC_TYPE)
    {
        Children.Add(child);
    }

    public HtmlDocument(Func<HtmlElement>[] children)
    {
        Children.AddRange(children);
    }
}

public class Head : HtmlElement
{
    public override string Tag => "head";

    public override bool SelfClosing => false;

    public Head(Func<HtmlElement> child) => this.Children.Add(child);

    public Head() { }
}

public class HtmlTag : HtmlElement
{
    public override string Tag { get; }

    public HtmlTag(string tag, Func<HtmlElement> child) { 
        Tag = tag;
        Children.Add(child);
    }

    HtmlTag () {}
}

public class HtmlText : HtmlElement
{
    private readonly string Text;

    public override string Tag => throw new NotImplementedException();

    public override string html() => Text;

    public HtmlText(string text) => Text = text;

}

public class Title : HtmlTag
{
    public Title(Func<HtmlText> child) : base("title", child)
    {
        this.Children.Add(child);
    }
}

public static class html
{
    public static Func<HtmlDocument> document() => () => new HtmlDocument();
    public static Func<HtmlDocument> document(Func<HtmlElement> child)
    {
        return () => new HtmlDocument(child);
    }

    public static Func<HtmlDocument> document(params Func<HtmlElement>[] children)
    {
        return () => new HtmlDocument(children);
    }    

    public static Func<Head> head() => () => new Head();
    public static Func<Head> head(Func<HtmlElement> child) => () => new Head(child);

    public static Func<Title> title() => () => new Title(() => new HtmlText(string.Empty));
    public static Func<Title> title(string title) => () => new Title(() => new HtmlText(title));
    public static Func<Title> text(string text) => () => new Title(() => new HtmlText(text));

    public static Func<HtmlTag> tag(string tag, Func<HtmlElement> child) => () => new HtmlTag(tag, child);
}