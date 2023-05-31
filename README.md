# SonicColorsXTBConv

## Description
This tool converts Sonic Colors' XTB files to XML and vice versa.

## Usage
```
SonicColorsXTBConv.exe <path to xtb or xml file>
```
Or you can just drop a XTB or XML file on the executable.

Converted XML file will look like this:
```xml
<?xml version="1.0" encoding="utf-8"?>
<XTB>
  <Styles>
    <Style size=15 id=CustomStyle horizontalAlignment="Left" colorR=255 colorG=255 colorB=255 />
  </Styles>
  <Categories>
    <Category name="CustomCategory">
      <Cell name=CustomCell style=CustomStyle>Text here. `$n` means \n by the way</Cell>
    </Category>
  </Categories>
</XTB>
```