[< Back](../readme.md)

## Format

You can format **Json** structure from compact to readable layout and back.

```json
{
    "body": {
        "value": 10
    }
}
```
> For this example **Json** text located in **Resources** folder in `data_0.json` file.

```cs
using UGF.Json.Runtime;
using UnityEngine;

namespace Example.Runtime
{
    public class ExampleFormat1 : MonoBehaviour
    {
        private void Start()
        {
            var textAsset = Resources.Load<TextAsset>("data_0");
            
            string compact = JsonTextUtility.ToCompact(textAsset.text);
            string readable = JsonTextUtility.ToReadable(compact);
            
            Debug.LogFormat("Compact: {0}", compact);
            Debug.LogFormat("Readable:\n{0}", readable);
            
            // Output:
            // Compact: {"body":{"value":10}}
            // Readable:
            // {
            //     "body": {
            //         "value": 10
            //     }
            // } 
        }
    }
}
```

Also you can use **JsonFormatter** class directly, to prevent memory allocation.

```cs
using UGF.Json.Runtime;
using UnityEngine;

namespace Example.Runtime
{
    public class ExampleFormat2 : MonoBehaviour
    {
        private readonly JsonFormatter m_formatter = new JsonFormatter();

        private void Start()
        {
            var textAsset = Resources.Load<TextAsset>("data_0");
            
            string compact = m_formatter.ToCompact(textAsset.text);
            string readable = m_formatter.ToReadable(compact);
            
            Debug.LogFormat("Compact: {0}", compact);
            Debug.LogFormat("Readable:\n{0}", readable);
            
            // Output:
            // Compact: {"body":{"value":10}} 
            // Readable:
            // {
            //     "body": {
            //         "value": 10
            //     }
            // }
        }
    }
}
```

And you can use **JsonFormatter** to escape and unescape special characters in a string.

```cs
using UGF.Json.Runtime;
using UnityEngine;

namespace Example.Runtime
{
    public class ExampleFormat3 : MonoBehaviour
    {
        private void Start()
        {
            string text = "\"C:\\Projects\\MyUnityProject\\Assets\\example.json\"";
            string escaped = JsonTextUtility.Escape(text);
            string unescaped = JsonTextUtility.Unescape(escaped);

            Debug.LogFormat("Text: {0}", text);
            Debug.LogFormat("Escaped: {0}", escaped);
            Debug.LogFormat("Unescaped: {0}", unescaped);

            // Output
            // Text: "C:\Projects\MyUnityProject\Assets\example.json"
            // Escaped: \"C:\\Projects\\MyUnityProject\\Assets\\example.json\"
            // Unescaped: "C:\Projects\MyUnityProject\Assets\example.json"
        }
    }
}
```

Or using it directly, to prevent memory allocation.

```cs
using UGF.Json.Runtime;
using UnityEngine;

namespace Example.Runtime
{
    public class ExampleFormat4 : MonoBehaviour
    {
        private readonly JsonFormatter m_formatter = new JsonFormatter();

        private void Start()
        {
            string text = "\"C:\\Projects\\MyUnityProject\\Assets\\example.json\"";
            string escaped = m_formatter.Escape(text);
            string unescaped = m_formatter.Unescape(escaped);

            Debug.LogFormat("Text: {0}", text);
            Debug.LogFormat("Escaped: {0}", escaped);
            Debug.LogFormat("Unescaped: {0}", unescaped);

            // Output
            // Text: "C:\Projects\MyUnityProject\Assets\example.json"
            // Escaped: \"C:\\Projects\\MyUnityProject\\Assets\\example.json\"
            // Unescaped: "C:\Projects\MyUnityProject\Assets\example.json"
        }
    }
}
```

---
> Unity Game Framework | Alexander Vorobyov | Copyright 2018