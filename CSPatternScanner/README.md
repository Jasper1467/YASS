# README

CSPatternScanner takes an input json file and generates a C-style header file for including in compilable Cpp projects.

```
Usage:
CSPatternScanner.exe -f <filetoscan> -s <signaturesfile> -o <outputfile>
```


## Input
```test.json
[{
	name: "chatPacket",
	pattern: "6a 5b",
	comment: ""
},
{
	name: "engine",
	pattern: "55 ?? 6B",
	comment: "Pointer to engine class"
}]
```

## Output

```dransik.h
#define chatPacket 0x5555
#define engine 0x6666 // Pointer to engine class
```