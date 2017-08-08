# Scheme.NET

**This is very much an in progress project.**

A lexer, parser, and interpreter for R5RS Scheme, for .NET.

Some, but not much, care has gone into ensuring compatibility with R5RS, but some things have been intentionally omitted for ease of implementation, and sometimes laziness.

- Performant arithmetic. All arithmetic/numbers are represented with arbitrary precision rationals, and as such are MUCH slower than IEEE floating point.
- Most procedures. I am slowly implementing more or less everything from R5RS, but most things are not in yet.

## What is this?

- A simple, yet powerful way to add basic scripting to applications using Scheme.

## What is this not?

- Performant (probably never). 
- Useful (yet). 
- Intended to be used in production (yet).
- A way to run Scheme as if it were a .NET language. (See IronScheme)

# Contributing

Just submit a pull request. If your code sucks I'll ask you to make it not suck. If it doesn't suck, and aligns with the goals/structure of the project (and includes tests), then sure!