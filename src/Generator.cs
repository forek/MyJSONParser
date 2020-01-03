using System;
using System.Collections;

namespace MyJSONParser.Core {
  public class Generator {
    public delegate void TraverseCallback(JSONElement member);

    static public Object Generate(JSONElement el) {
      if (el.type == "array") {
        return TraverseArray(el);
      } else if (el.type == "object") {
        return TraverseObject(el);
      }
      return null;
    }

    static public Hashtable TraverseObject(JSONElement el) {
      var obj = new Hashtable();
      string key = null;
      foreach (var item in el.child) {
        if (item.scope == "key") {
          key = item.value;
        } else if (item.scope == "value") {
          switch (item.type) {
            case "nil":
              obj.Add(key, null);
              break;
            case "s":
              obj.Add(key, item.value);
              break;
            case "t":
              obj.Add(key, true);
              break;
            case "f":
              obj.Add(key, false);
              break;
            case "n":
              {
                double number;
                Double.TryParse(item.value, out number);
                obj.Add(key, number);
                break;
              }
            case "array":
              obj.Add(key, TraverseArray(item));
              break;
            case "object":
              obj.Add(key, TraverseObject(item));
              break;
            default:
              break;
          }
        }
      }

      return obj;
    }

    static public ArrayList TraverseArray(JSONElement el) {
      var arr = new ArrayList();

      foreach (var item in el.child) {
        switch (item.type) {
          case "nil":
            arr.Add(null);
            break;
          case "s":
            arr.Add(item.value);
            break;
          case "t":
            arr.Add(true);
            break;
          case "f":
            arr.Add(false);
            break;
          case "n":
            {
              double number;
              Double.TryParse(item.value, out number);
              arr.Add(number);
              break;
            }
          case "array":
            arr.Add(TraverseArray(item));
            break;
          case "object":
            arr.Add(TraverseObject(item));
            break;
          default:
            break;
        }
      }

      return arr;
    }
  }
}