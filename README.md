[![Version](https://img.shields.io/static/v1?label=version&message=Taf.Api 1.0.0 · Taf.Core 3.0.0 · Backend 2.0.0 · UI.Core 3.0.0 · UI.Web 3.0.0 · UI.Win 3.0.0&color=blue&style=plastic)](https://bitbucket.org/dobriyanchik/unicorntaf/src/unicorn-2.3.0/)  
[![Licence](https://img.shields.io/static/v1?label=license&message=Apache-2.0&color=white&style=plastic)](https://www.apache.org/licenses/LICENSE-2.0)
[![Issues](https://img.shields.io/bitbucket/issues/dobriyanchik/unicorntaf?style=plastic)](https://bitbucket.org/dobriyanchik/unicorntaf/issues?status=new&status=open)
[![Pipelines](https://img.shields.io/bitbucket/pipelines/dobriyanchik/unicorntaf/master?style=plastic)](https://bitbucket.org/dobriyanchik/unicorntaf/pipelines)
[![Coverage](https://img.shields.io/static/v1?label=coverage&message=58%&color=yellow&style=plastic)](https://bitbucket.org/dobriyanchik/unicorntaf/src/master/)

Unicorn test automation framework
================================

Content
-------

The following files and directories are presented:

      example/         code with TAF usage examples
	  src/             framework core sources
	  LICENSE.txt	   licence file
	  README.md        this file


TAF Solution Structure
=====================

Unicorn.Taf.Api
---------------
common API for core of unicorn automation framework.

* common APIs
* AppDomain isolation
* Unicorn AssemblyLoadContext

Unicorn.Taf.Core
---------------
Core of unicorn automation framework.

* Unit test framework implementation
* Logger abstractions
* Base utilities
* Test steps instrumentation

Unicorn.Backend
--------------
Implementation of clients for interaction with services based on `HttpClient`
* Client for REST services
* REST service matchers collection

> TBD: Client for SOAP services  
> TBD: batabase client

Unicorn.UI.Core
--------------
UI core of unicorn automation framework.

* Driver, PageObject and search context abstractions
* UI matchers

Unicorn.UI.Win
-------------
Implementation of desktop GUI interaction based on UIA3 library.

* GUI Driver implementation
* Typified controls implementations
* PageObject implementation
* UI actions
* User input clients (mouse, keyboard)

Unicorn.UI.Web
-------------
Implementation of browser interaction based on Selenium.

* Web Driver implementation
* Typified controls implementations
* PageObject implementation
* abstract WebSite and pages pool


Unicorn.UI.Mobile
----------------
Implementation of Mobile UI interaction which is based on Appium.

* Web Driver implementation (iOS, Android)
* Typified controls implementations (iOS, Android)
* PageObject implementation (iOS, Android)