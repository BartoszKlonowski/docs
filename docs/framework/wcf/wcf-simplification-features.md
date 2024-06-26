---
description: "Learn more about: WCF Simplification Features"
title: "WCF Simplification Features"
ms.date: "03/30/2017"
ms.assetid: 4535a511-6064-4da0-b361-80262a891663
---

# WCF Simplification Features

This topic discusses new features that make writing WCF applications simpler.

[!INCLUDE[](~/includes/wcf_grpc_heading.md)]

## Simplified Generated Configuration Files

When you add a service reference in Visual Studio or use the SvcUtil.exe tool a client configuration file is generated. In previous versions of WCF these configuration files contained the value of every binding property even if its value is the default value. In WCF 4.5 the generated configuration files contain only those binding properties that are set to a non-default value.

The following is an example of a configuration file generated by WCF 3.0.

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
    <system.serviceModel>
        <bindings>
            <basicHttpBinding>
                <binding name="BasicHttpBinding_IService1" closeTimeout="00:01:00"
                    openTimeout="00:01:00" receiveTimeout="00:10:00" sendTimeout="00:01:00"
                    allowCookies="false" bypassProxyOnLocal="false"
                    hostNameComparisonMode="StrongWildcard" maxBufferSize="65536"
                    maxBufferPoolSize="524288" maxReceivedMessageSize="65536"
                    messageEncoding="Text" textEncoding="utf-8" transferMode="Buffered"
                    useDefaultWebProxy="true">
                    <readerQuotas maxDepth="32" maxStringContentLength="8192"
                        maxArrayLength="16384" maxBytesPerRead="4096"
                        maxNameTableCharCount="16384" />
                    <security mode="None">
                        <transport clientCredentialType="None" proxyCredentialType="None"
                            realm="" />
                        <message clientCredentialType="UserName" algorithmSuite="Default" />
                    </security>
                </binding>
            </basicHttpBinding>
        </bindings>
        <client>
            <endpoint address="http://localhost:36906/Service1.svc" binding="basicHttpBinding"
                bindingConfiguration="BasicHttpBinding_IService1" contract="IService1"
                name="BasicHttpBinding_IService1" />
        </client>
    </system.serviceModel>
</configuration>
```

The following is an example of the same configuration file generated by WCF 4.5.

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
    <system.serviceModel>
        <bindings>
            <basicHttpBinding>
                <binding name="BasicHttpBinding_IService1" />
            </basicHttpBinding>
        </bindings>
        <client>
            <endpoint address="http://localhost:36906/Service1.svc" binding="basicHttpBinding"
                bindingConfiguration="BasicHttpBinding_IService1" contract="IService1"
                name="BasicHttpBinding_IService1" />
        </client>
    </system.serviceModel>
</configuration>
```

## Contract-First Development

WCF now has support for contract-first development. The svcutil.exe tool has a /serviceContract switch which allows you to generate service and data contracts from a WSDL document.

## Add Service Reference From a Portable Subset Project

Portable subset projects enable .NET assembly programmers to maintain a single source tree and build system while still supporting multiple .NET implementations (desktop, Silverlight, Windows Phone, and Xbox). Portable subset projects only reference .NET portable libraries that are assemblies that can be used on any .NET implementation. The developer experience is the same as adding a service reference within any other WCF client application. For more information, see [Add Service Reference in a Portable Subset Project](add-service-reference-in-a-portable-subset-project.md).

## ASP.NET Compatibility Mode Default Changed

WCF provides ASP.NET compatibility mode to grant developers full access to the features in the ASP.NET HTTP pipeline when writing WCF services. To use this mode, you must set the `aspNetCompatibilityEnabled` attribute to true in the [\<serviceHostingEnvironment>](../configure-apps/file-schema/wcf/servicehostingenvironment.md) section of web.config. Additionally, any service in this appDomain needs to have the `RequirementsMode` property on its <xref:System.ServiceModel.Activation.AspNetCompatibilityRequirementsAttribute> set to <xref:System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed> or <xref:System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Required>. By default <xref:System.ServiceModel.Activation.AspNetCompatibilityRequirementsAttribute> is now set to <xref:System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed> and the default WCF service application template sets the `aspNetCompatibilityEnabled` attribute to `true`. For more information, see [What's New in Windows Communication Foundation 4.5](whats-new.md) and [WCF Services and ASP.NET](./feature-details/wcf-services-and-aspnet.md).

## Streaming Improvements

- New support for asynchronous streaming has been added to WCF. To enable asynchronous streaming, add the  <xref:System.ServiceModel.Description.DispatcherSynchronizationBehavior> endpoint behavior to the service host and set its <xref:System.ServiceModel.Description.DispatcherSynchronizationBehavior.AsynchronousSendEnabled%2A> property to `true`. This can benefit scalability when a service is sending streamed messages to multiple clients which are reading slowly. WCF does not block one thread per client anymore and will free up the thread to service another client.

- Removed limitations around buffering of messages when a service is IIS hosted. In previous versions of WCF when receiving a message for an IIS-hosted service that used streaming message transfer, ASP.NET would buffer the entire message before sending it to WCF. This would cause large memory consumption. This buffering has been removed in .NET Framework 4.5 and now IIS-hosted WCF services can start processing the incoming stream before the entire message has been received, thereby enabling true streaming. This allows WCF to respond immediately to messages and allows improved performance. In addition, you no longer have to specify a value for `maxRequestLength`, the ASP.NET size limit on incoming requests. If this property is set, it is ignored. For more information about `maxRequestLength` see [\<httpRuntime> configuration element](/previous-versions/dotnet/netframework-1.1/e1f13641(v=vs.71)). You will still need to configure the maxAllowedContentLength, For more information, see [IIS Request Limits](/previous-versions/iis/settings-schema/ms689462(v=vs.90)).

## New Transport Default Values

The following table describes the settings that have changed and where to find additional information.

|Property|On|New Default|More Information|
|--------------|--------|-----------------|----------------------|
|channelInitializationTimeout|<xref:System.ServiceModel.NetTcpBinding>|30 seconds|This property determines how long a TCP connection can take to authenticate itself using the .NET Framing protocol. A client needs to send some initial data before the server has enough information to perform authentication. This timeout is intentionally made smaller than the ReceiveTimeout (10 min) so that malicious unauthenticated clients do not keep the connections tied up to the server for long. The default value is 30 seconds. For more information about <xref:System.ServiceModel.Channels.ConnectionOrientedTransportBindingElement.ChannelInitializationTimeout%2A>|
|listenBacklog|<xref:System.ServiceModel.NetTcpBinding>|16 * number of processors|This socket-level property describes the number of "pending accept" requests to be queued. If the listen backlog queue fills up, new socket requests will be rejected. For more information about <xref:System.ServiceModel.NetTcpBinding.ListenBacklog%2A>|
|maxPendingAccepts|ConnectionOrientedTransportBindingElement<br /><br /> SMSvcHost.exe|2 * number of processors for transport<br /><br /> 4 \* number of processors for SMSvcHost.exe|This property limits the number of channels that the server can have waiting on a listener. When MaxPendingAccepts is too low, there will be a small interval of time in which all of the waiting channels have started servicing connections, but no new channels have begun listening. A connection can arrive during this interval and will fail because nothing is waiting for it on the server. This property can be configured by setting the <xref:System.ServiceModel.Channels.ConnectionOrientedTransportBindingElement.MaxPendingConnections%2A> property to a larger number. For more information, see <xref:System.ServiceModel.Channels.ConnectionOrientedTransportBindingElement.MaxPendingAccepts%2A> and [Configuring the Net.TCP Port Sharing Service](./feature-details/configuring-the-net-tcp-port-sharing-service.md)|
|maxPendingConnections|ConnectionOrientedTransportBindingElement|12 * number of processors|This property controls how many connections a transport has accepted but have not been picked up by the ServiceModel Dispatcher. To set this value, use `MaxConnections` on the binding or `maxOutboundConnectionsPerEndpoint` on the binding element. For more information about <xref:System.ServiceModel.Channels.ConnectionOrientedTransportBindingElement.MaxPendingConnections%2A>|
|receiveTimeout|SMSvcHost.exe|30 seconds|This property specifies the timeout for reading the TCP framing data and performing connection dispatching from the underlying connections. This exists to put a cap on the amount of time SMSvcHost.exe service is kept engaged to read the preamble data from an incoming connection. For more information, see [Configuring the Net.TCP Port Sharing Service](./feature-details/configuring-the-net-tcp-port-sharing-service.md).|

> [!NOTE]
> These new defaults are used only if you deploy the WCF service on a machine with .NET Framework 4.5. If you deploy the same service on a machine with .NET Framework 4.0, then the .NET Framework 4.0 defaults are used. In such cases it is recommended to configure these settings explicitly.

## XmlDictionaryReaderQuotas

<xref:System.Xml.XmlDictionaryReaderQuotas> contains configurable quota values for XML dictionary readers which limit the amount of memory utilized by an encoder while creating a message. While these quotas are configurable, the default values have changed to lessen the possibility that a developer will need to set them explicitly. `MaxReceivedMessageSize` quota has not been changed so that it can still limit memory consumption preventing the need for you to deal with the complexity of the <xref:System.Xml.XmlDictionaryReaderQuotas>. The following table shows the quotas, their new default values and a brief explanation of what each quota is used for.

|Quota Name|Default Value|Description|
|----------------|-------------------|-----------------|
|<xref:System.Xml.XmlDictionaryReaderQuotas.MaxArrayLength%2A>|Int32.MaxValue|Gets and sets the maximum allowed array length. This quota limits the maximum size of an array of primitives that the XML reader returns, including byte arrays. This quota does not limit memory consumption in the XML reader itself, but in whatever component that is using the reader. For example, when the <xref:System.Runtime.Serialization.DataContractSerializer> uses a reader secured with <xref:System.Xml.XmlDictionaryReaderQuotas.MaxArrayLength%2A>, it does not deserialize byte arrays larger than this quota.|
|<xref:System.Xml.XmlDictionaryReaderQuotas.MaxBytesPerRead%2A>|Int32.MaxValue|Gets and sets the maximum allowed bytes returned for each read. This quota limits the number of bytes that are read in a single Read operation when reading the element start tag and its attributes. (In non-streamed cases, the element name itself is not counted against the quota). Having too many XML attributes may use up disproportionate processing time because attribute names have to be checked for uniqueness. <xref:System.Xml.XmlDictionaryReaderQuotas.MaxBytesPerRead%2A> mitigates this threat.|
|<xref:System.Xml.XmlDictionaryReaderQuotas.MaxDepth%2A>|128 nodes deep|This quota limits the maximum nesting depth of XML elements. <xref:System.Xml.XmlDictionaryReaderQuotas.MaxDepth%2A> interacts with <xref:System.Xml.XmlDictionaryReaderQuotas.MaxBytesPerRead%2A>: the reader always keeps data in memory for the current element and all of its ancestors, so the maximum memory consumption of the reader is proportional to the product of these two settings. When deserializing a deeply-nested object graph, the deserializer is forced to access the entire stack and throw an unrecoverable <xref:System.StackOverflowException>. A direct correlation exists between XML nesting and object nesting for both the <xref:System.Runtime.Serialization.DataContractSerializer> and the <xref:System.Xml.Serialization.XmlSerializer>. <xref:System.Xml.XmlDictionaryReaderQuotas.MaxDepth%2A> is used to mitigate this threat.|
|<xref:System.Xml.XmlDictionaryReaderQuotas.MaxNameTableCharCount%2A>|Int32.MaxValue|This quota limits the maximum number of characters allowed in a nametable. The nametable contains certain strings (such as namespaces and prefixes) that are encountered when processing an XML document. As these strings are buffered in memory, this quota is used to prevent excessive buffering when streaming is expected.|
|<xref:System.Xml.XmlDictionaryReaderQuotas.MaxStringContentLength%2A>|Int32.MaxValue|This quota limits the maximum string size that the XML reader returns. This quota does not limit memory consumption in the XML reader itself, but in the component that is using the reader. For example, when the <xref:System.Runtime.Serialization.DataContractSerializer> uses a reader secured with <xref:System.Xml.XmlDictionaryReaderQuotas.MaxStringContentLength%2A>, it does not deserialize strings larger than this quota.|

> [!IMPORTANT]
> Refer to "Using XML Safely" under [Security Considerations for Data](./feature-details/security-considerations-for-data.md) for more information about securing your data.

> [!NOTE]
> These new defaults are used only if you deploy the WCF service on a machine with .NET Framework 4.5. If you deploy the same service on a machine with .NET Framework 4.0, then the .NET Framework 4.0 defaults are used. In such cases it is recommended to configure these settings explicitly.

## WCF Configuration Validation

As part of the build process within Visual Studio, WCF configuration files are now validated. A list of validation errors or warnings are displayed in Visual Studio if the validation fails.

## XML Editor Tooltips

In order to help new and existing WCF service developers to configure their services, the Visual Studio XML editor now provides tooltips for every configuration element and its properties that is part of the service configuration file.

## BasicHttpBinding Improvements

1. Enables a single WCF endpoint to respond to different authentication modes.

2. Enables a WCF service’s security settings to be controlled by IIS
