# Scheme.NET

**This is very much an in progress project.**

A lexer, parser, and interpreter for R5RS Scheme, for .NET.

Some, but not much, care has gone into ensuring compatibility with R5RS, but some things have been intentionally omitted for ease of implementation, and sometimes laziness.

- The full numeric tower. All numbers are arbitrary precision rationals, which also means that everything is automatically exact. As such, there is no concept of exact vs inexact.
- Most procedures. I am slowly implementing more or less everything from R5RS, but most things are not in yet.
- Vectors. (Mostly laziness, this will be implemented soon).


## What is this?

- A simple, yet powerful way to add basic scripting to applications using Scheme.

## What is this not?

- Performant (probably never). 
- Useful (yet). 
- Intended to be used in production (yet).
- A way to run Scheme as if it were a .NET language. (See IronScheme)

## Intentionally unimplemented procedures

Note that this is not the complete list of unimplemented procedures. This is only the list of procedures that I deliberately skipped.

It is a safe bet that if it is defined in R5RS, I have not yet implemented it.

- complex?
- real?
- rational?
- exact?
- inexact?
- rationalize?
- exp
- log
- sin
- cos
- tan
- asin
- acos
- atan
- sqrt
- expt
- make-rectangular
- make-polar
- real-part
- imag-part
- magnitude
- angle
- exact->inexact
- inexact->exact

# Contributing

Just submit a pull request. If your code sucks I'll ask you to make it not suck. If it doesn't suck, and aligns with the goals/structure of the project (and includes tests), then sure!