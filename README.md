# StringExtractor

StringExtractor is a tool designed to extract user-created strings from .NET files (.exe/.dll) by leveraging the `#US` stream. Unlike traditional methods, it does not rely on enumerating `ldstr` instructions or accessing memory directly, ensuring a streamlined and efficient extraction process.

## Features
- **Stream-Based Extraction**: Operates primarily using the `#US` stream to retrieve strings.
- **Compatibility**: Supports both x86 and x64 architectures.
- **Obfuscated Strings**: If an assembly has encrypted strings (due to obfuscation) but the encrypted data is still present, the tool will extract the strings in their encrypted state.

## Requirements
- The target application **must** contain a `#US` stream, as the tool is dependent on this.
- Only compatible with **.NET files** (.exe/.dll).

## Building
To build for both x86 and x64 architectures simultaneously in Visual Studio, use the **Batch Build** feature. Configurations for both architectures are pre-configured.


TO-DO:
- [ ] Incooperate a string decryption mechanism
- [ ] Add a check to make sure the given file is .NET

![image](https://user-images.githubusercontent.com/52993096/144455858-780a6792-5346-440c-9eba-f8b01cce6ed7.png)



Credits:
Thank you to 0xd4d for the use of (https://github.com/0xd4d/dnlib)
