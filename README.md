StringExtractor

This program will allow for you to extract all user created strings from .NET files (.exe/.dll). It will not be using any enumeration for ldstr instructions as well as no memory access at all. This program is primarily using the #US stream to extract strings.

NOTE:
- This program will not operate at all if the target application does not contain a #US stream. (as it is based on that)
- Target applications must be .NET files only.
- If the target assembly has encrypted strings (from obfuscation),and the string is still present in the code it will be retrieved. (however in its encrypted state)
- Supports x86 and x64 bit architecture files.
- To build both architectures at the same time in Visual Studio, use batch build.(configurations are already made)


TO-DO:
- [ ] Incooperate a string decryption mechanism
- [ ] Add a check to make sure the given file is .NET

![image](https://user-images.githubusercontent.com/52993096/144455858-780a6792-5346-440c-9eba-f8b01cce6ed7.png)



Credits:
Thank you to 0xd4d for the use of (https://github.com/0xd4d/dnlib)
