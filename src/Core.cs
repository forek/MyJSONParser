using System;
using System.Collections.Generic;
using MyJSONParser.Tokenize;

namespace MyJSONParser.Core {

  public class Parser {
    static public void Parse(Tokens tokens) {
      var stack = new Stack<Object>();
      Console.WriteLine("Parsing ... ");
      while (tokens.ToArray().Length > 0) {
        var curr = tokens.Shift();
        // Console.WriteLine(curr);
      }
    }
  }
}