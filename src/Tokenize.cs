using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Parser {
  public class TokenType {
    public TokenType (string token, string pattern, ValueModifier? modifier) {
      this.token = token;
      this.pattern = pattern;
      this.modifier = modifier;
    }

    public string token;

    public string pattern;

    public delegate string ValueModifier (string s);

    public ValueModifier modifier;

    public override string ToString () {
      return $"[TokenType] token: {token}, pattern: {pattern}, modifier: {modifier}";
    }
  }

  public class Tokens : List<Token> {
    public override string ToString () {
      string result = "";
      foreach (var item in this) {
        result += $"[Token] type: {item.type} , value: {item.value} \n";
      }
      return result;
    }
  }

  public class Token {
    public string type;
    public string value;

    static public List<TokenType> Types = new List<TokenType> () {
      new TokenType ("s", "\".*?\"", null),
      new TokenType ("n", @"-?(0|([1-9][0-9]*))(\.\d*[1-9])?((e|E)(\+|\-)?\d+)?", null),
      new TokenType ("t", "true", null),
      new TokenType ("f", "false", null),
      new TokenType ("nil", "null", null),
      new TokenType ("{", "{", null),
      new TokenType ("}", "}", null),
      new TokenType ("[", @"\[", null),
      new TokenType ("]", @"\]", null),
      new TokenType (",", ",", null),
      new TokenType (":", ":", null),
      new TokenType ("space", @"\s", null)
    };

    static public Tokens Tokenize (string input) {
      string str = input.Clone () as string;
      var result = new Tokens ();
      while (str.Length > 0) {
        bool hasToken = false;

        foreach (var t in Types) {
          Match mc = Regex.Match (str, $"^{t.pattern}");
          if (mc.Equals (Match.Empty)) {
            continue;
          } else {
            result.Add (new Token () { type = t.token, value = mc.ToString () });
            str = str.Substring (mc.Length);
            Console.WriteLine ($"[eat]: {t.token} {mc} ");
            hasToken = true;
            break;
          }
        }

        if (!hasToken) {
          Console.WriteLine ("error");
          break;
        }
      }

      return result;
    }

    static public void Log () {
      foreach (TokenType item in Types) {
        Console.WriteLine (item);
      }
    }
  }
}