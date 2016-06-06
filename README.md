# RobotToolkit
Tools for Robot Structural Analysis


### Build RobotToolkit from Source ###
You will need the following to build Basilisk:

- Microsoft Visual Studio 2013 or higher
- [GitHub for Windows](https://windows.github.com/)
- Microsoft .NET Framework 4.0 and above (included with Visual Studio 2013)
- BHoM version 0.0.1
- Ensure post-build folders are accessible:
 
## Contribute ##

RobotToolkit is a BuroHappold open-source project and would be nothing without its community.  You can submit your own code to the RobotToolkit project via a Github [pull request](https://help.github.com/articles/using-pull-requests).

## Releases ##
###0.0.1 ###
Known Issues
 - A lot of the code is legacy utilities code that circumvents the BHoM in favour of lists and arrays of numbers. We're in the process of switching the input/output to BHoM objects, so please note that functional
 programming methods currently called in your code will definitely break in future releases. Bear with us..!
 
Bug fixes
 - Visibility of Robot when accessing bars by filename is fixed

New features
 - Bar force extraction and conversion to BHoM bar force added

## License ##

RobotToolkit is licensed under the Apache License. RobotToolkit also uses a number of third party libraries, some with different licenses.

