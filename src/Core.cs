using System;
using System.Collections.Generic;
using MyJSONParser.Tokenize;

namespace MyJSONParser.Core {
  using JSONElementList = List<JSONElement>;

  public class JSONElement {
    public string type;
    public string value;
    public string scope = null;
    public JSONElementList child;
    public JSONElement parent;

    public JSONElement(string type) {
      this.type = type;
      this.parent = null;
      this.value = null;
      this.child = new JSONElementList();
    }

    public JSONElement(string type, JSONElement parent, string value = null) {
      this.type = type;
      this.parent = parent;
      this.value = value;
      this.child = new JSONElementList();
    }

    public JSONElement(string type, JSONElement parent, string value, string scope) {
      this.type = type;
      this.parent = parent;
      this.value = value;
      this.child = new JSONElementList();
      this.scope = scope;
    }

    public void AddChild(JSONElement el) {
      child.Add(el);
    }

    public JSONElement GetLastChild() {
      if (child.ToArray().Length == 0) return null;
      return child[child.ToArray().Length - 1];
    }

    public string ToString(int deep) {
      string prefix = "";
      for (int i = 0; i < deep; i++) {
        prefix += "-";
      }

      string result = $"{prefix} type: {this.type} | value: {(this.value == null ? "empty" : this.value)} | scope: {(this.scope == null ? "empty" : this.scope)}\n";

      foreach (var item in child) {
        result += item.ToString(deep + 1);
      }

      return result;
    }

    public override string ToString() {
      return this.ToString(1);
    }
  }

  public class Parser {
    static private void ThrowError(Token t) {
      throw (new ApplicationException($"Unexpected token: {t}"));
    }

    static public JSONElement Parse(Tokens tokens) {
      var result = new JSONElement("root");
      JSONElement p = result;

      while (tokens.ToArray().Length > 0) {
        var curr = tokens.Shift();
        switch (curr.type) {
          case "{":
            {
              var node = new JSONElement("object", p, null, "value");
              p.AddChild(node);
              p = node;
              break;
            }

          case "[":
            {
              var node = new JSONElement("array", p, null, "value");
              p.AddChild(node);
              p = node;
              break;
            }
          case "}":
            {
              var lastChild = p.GetLastChild();
              if (
                p.type == "object" &&
                (lastChild == null || lastChild.scope == "value")
              ) {
                p = p.parent;
              } else {
                ThrowError(curr);
              }
              break;
            }
          case "]":
            {
              var lastChild = p.GetLastChild();
              if (
                p.type == "array" &&
                (lastChild == null || lastChild.scope == "value")
              ) {
                p = p.parent;
              } else {
                ThrowError(curr);
              }
              break;
            }

          case "s":
            {
              var lastChild = p.GetLastChild();
              if (p.type == "object") {
                if (lastChild == null || lastChild.type == ",") {
                  p.AddChild(new JSONElement(curr.type, p, curr.value, "key"));
                } else if (lastChild != null && lastChild.type == ":") {
                  p.AddChild(new JSONElement(curr.type, p, curr.value, "value"));
                } else {
                  ThrowError(curr);
                }
              } else if (p.type == "array") {
                if (lastChild == null || lastChild.type == ",") {
                  p.AddChild(new JSONElement(curr.type, p, curr.value, "value"));
                } else {
                  ThrowError(curr);
                }
              }
              break;
            }
          case "n":
          case "t":
          case "f":
          case "nil":
            {
              var lastChild = p.GetLastChild();
              if (
                (p.type == "object" && (lastChild != null && lastChild.type == ":")) ||
                (p.type == "array" && (lastChild == null || lastChild.type == ","))
              ) {
                p.AddChild(new JSONElement(curr.type, p, curr.value, "value"));
              } else {
                ThrowError(curr);
              }
              break;
            }
          case ",":
            {
              var lastChild = p.GetLastChild();
              if (lastChild != null && lastChild.scope == "value") {
                p.AddChild(new JSONElement(curr.type, p, curr.value));
              } else {
                ThrowError(curr);
              }
              break;
            }
          case ":":
            {
              var lastChild = p.GetLastChild();
              if (lastChild != null && lastChild.scope == "key") {
                p.AddChild(new JSONElement(curr.type, p, curr.value));
              } else {
                ThrowError(curr);
              }
              break;
            }
          case "space":
            {
              break;
            }
          default:
            {
              ThrowError(curr);
              break;
            }
        }
      }

      if (result != p) throw (new ApplicationException($"Unexpected token"));

      return result.child[0];
    }
  }
}