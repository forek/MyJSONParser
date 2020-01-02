using System;
using MyJSONParser.Tokenize;
using MyJSONParser.Core;

namespace MyJSONParser {
  class Program {
    static void Main(string[] args) {
      string str = "{ \"abc\": 123 }";

      var tokens = Token.Tokenize(str);
      Parser.Parse(tokens);
      // Console.WriteLine(tokens);
    }
  }
}