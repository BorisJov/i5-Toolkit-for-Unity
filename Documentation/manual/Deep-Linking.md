# Deep Linking

![Deep Linking](../resources/Logos/DeepLink.svg)

## Mobile Deep Links

Mobile deep links are special links on Web pages.
They allow users to seamlessly switch into an installed application and trigger an action there.
These deep links do not start with "http://" or "https://" but use a custom schema that can be freely chosen.
For instance, it is possible to register an app to react to all links that start with "i5://".
To ensure that the schema is unique, it is also possible to use reverse DNS notation, e.g. by writing "com.i5.Toolkit://".

For more information on mobile deep links can be found [here](https://en.wikipedia.org/wiki/Mobile_deep_linking).

## Use Case

The mobile deep links can be used to quickly access functionalities and services of installed applications.
As an example, it is possible to tell the app to immediately load certain content or open a specific editor.
A common deep link that can be found on the Web is the "mailto://" schema.
If a user clicks the link, the mail client opens and automatically sets up a new mail where the receiver mail is automatically filled based on the link's content.

## Usage

Follow these steps to integrate deep linking into your application:

1. Add a <xref:i5.Toolkit.Core.DeepLinkAPI.DeepLinkingService> to your application, e.g. in a [service bootstrapper](Service-Core.md#bootstrappers).
   
   ```[C#]
   DeepLinkingService service = new DeepLinkingService();
   ServiceManager.RegisterService(service);
   ```
2. Add the <xref:i5.Toolkit.Core.DeepLinkAPI.DeepLinkAttribute> to a method that should react to a deep link.
   When specifying the attribute, set the path to which it should react, e.g. "myDeepLink" if it should react to deep links like "i5://myDeepLink".
   The path is case-insensitive.

   ```[C#]
   [DeepLink("myPath")]
   public void Foo()
   { ... }
   ```
3. *Important*: To optimize performance, the <xref:i5.Toolkit.Core.DeepLinkAPI.DeepLinkingService> does not scan the entire code for the methods with attributes.
   Instead, you need to add the class that contains the method manually using <xref:i5.Toolkit.Core.DeepLinkAPI.DeepLinkingService.AddDeepLinkListener(System.Object)>.

   ```[C#]
   service.AddDeepLinkListener(myClass);
   ```
4. To clean up, you can remove a listener class again using <xref:i5.Toolkit.Core.DeepLinkAPI.DeepLinkingService.RemoveDeepLinkListener(System.Object)>

### Filtering Schemas

Optionally, you can also enter a schema in the attribute's definition.
If you do not add a schema, all schemas are recognized which have the same path.
For instance, `[DeepLink("myDeepLink")]` will be activated by any URL with the path myDeepLink, e.g. "i5://myDeepLink" but also "rwth://myDeepLink", etc.
If the schema is specified, only links which match this exact schema target the given method.
So, `[DeepLink(schema: "i5", path: "myDeepLink")]` will only be called by the deep link "i5://myDeepLink" but e.g. not by "rwth://myDeepLink".

### Extracting Further Information About the Received Deep Link

Methods that are marked with the <xref:i5.Toolkit.Core.DeepLinkAPI.DeepLinkAttribute> can either take no arguments or one argument of type <xref:i5.Toolkit.Core.DeepLinkAPI.DeepLinkArgs>.
In this argument helper class, the full deep link can be found, as well as information about the schema, and parameters that were specified with the deep link.

#### Passing Parameters to the Application Using Deep Links

You can specify parameters in the deep link, e.g. "i5://withParams?value=123&secondvalue=helloWorld".
They are available as a <xref:System.Collections.Generic.Dictionary`2> in the <xref:i5.Toolkit.Core.DeepLinkAPI.DeepLinkArgs.Parameters>.

### Recommendations

It is possible to mark any method in the code as a deep link receiver.
However, you should only choose methods that are available after the automatic initialization procedure of your application.
Moreover, the methods should stay available during the entire lifetime of the application.

This is due to the fact that the deep links should be state-free.
No matter where in the application you are, a received deep link should always have the same effect.
Moreover, when launching the application via a deep link, it should directly react to the deep link's content without user intervention.
It would be confusing for users if they start via a deep link, get into the normal main menu, can interact with it and e.g. only if they open the settings menu, the deep link is suddenly recognized and has an effect.

To keep the architecture clean, it is recommended to create few API-definition classes that bundle deep link paths instead of scattering them throughout the application's code.
These API-definition classes should be available from the beginning of the application and should persist until the application is terminated.

## Example Scene

There is an example scene that demonstrates the usage of the deep links.
It can only be tested in UWP, Android and iOS builds.

Follow these steps to set up the example:
1. Open the example scene "Deep Linking Demo".
2. Go to "File > Build Settings" and click the button "Add open scenes".
   Make sure that the "Deep Linking Demo" scene has index 0 in the build - it needs to be at the top of the list.
   If it is not at the top, you can drag and drop the scene entry so that it is the first one in the list.
3. In the build settings dialog, select either Universal Windows Platform (UWP), Android or iOS as the target platform and click the "Switch Platform" button.
   If you want to test on your development Windows PC, it is recommended to choose UWP because you can directly install the app on your PC and do not need an additional smartphone.
4. Click the "Player Settings" button in the build settings dialog.
   After that, register a deep link schema for "i5" for your selected target platform.
5. Build the application and install it on your device.
6. Click on the following link on the device on which you installed the application.
   The browser will ask you whether you want to open the link with your application.

<i5://changeColor?color=#0000ff>

The cube in the scene has a deep link receiver on it that responds to the *changeColor* path.
You can modify the value of the *color* parameter by copying the link and manually pasting it into the browser's address bar.
Each time you hit enter, the browser redirects to the example application and the cube changes its color.
This works both, if the app is not running in the background and with the already opened app.

If you hit F5, you can toggle the visibility of a fullscreen [app console](App-Console.md) which prints the deep link that was received.