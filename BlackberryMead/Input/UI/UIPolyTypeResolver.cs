using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace BlackberryMead.Input.UI
{
    /// <summary>
    /// Resolves polymorphism dependencies when deserializing REngine UI from JSON.
    /// </summary>
    /// <remarks>
    /// To extend deserialization to custom derived types a <see cref="JsonSerializerOptions.TypeInfoResolver"/> 
    /// must be used. To do so, create a blank class that derives from <see cref="DefaultJsonTypeInfoResolver"/>
    /// and copy <see cref="GetTypeInfo(Type, JsonSerializerOptions)"/> into it. Then edit the DerivedTypes field of
    /// the <see cref="JsonPolymorphismOptions"/> to include the desired 
    /// <see cref="JsonDerivedType"/>.
    /// </remarks>

    public class UIPolyTypeResolver : DefaultJsonTypeInfoResolver
    {
        public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
        {
            JsonTypeInfo jsonTypeInfo = base.GetTypeInfo(type, options);

            Type baseType = typeof(UIComponent);
            if (jsonTypeInfo.Type == baseType)
            {
                jsonTypeInfo.PolymorphismOptions = new JsonPolymorphismOptions
                {
                    IgnoreUnrecognizedTypeDiscriminators = true,
                    UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization,
                    DerivedTypes = {
                        new JsonDerivedType(typeof(TabViewer), typeDiscriminator: "TabViewer"),
                        new JsonDerivedType(typeof(ScrollBar), typeDiscriminator: "ScrollBar"),
                        new JsonDerivedType(typeof(ScrollViewer), typeDiscriminator: "ScrollViewer"),
                        new JsonDerivedType(typeof(Label), typeDiscriminator: "Label"),
                        new JsonDerivedType(typeof(Button), typeDiscriminator: "Button"),
                        new JsonDerivedType(typeof(Window), typeDiscriminator: "Window"),
                        new JsonDerivedType(typeof(ExtendoBox), typeDiscriminator: "ExtendoBox"),
                        new JsonDerivedType(typeof(Group), typeDiscriminator: "Group"),
                        new JsonDerivedType(typeof(UIImage), typeDiscriminator: "Image")
                    }
                };
            }

            return jsonTypeInfo;
        }
    }
}