[< Back](../readme.md)

## Read

Example of how to access to specific member of **Json** structure.

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
    public class ExampleRead1 : MonoBehaviour
    {
        private void Start()
        {
            var textAsset = Resources.Load<TextAsset>("data_0");

            var member = JsonTextUtility.Read(textAsset.text);
            var body = member.Find("body");
            var value = body.Find("value");

            Debug.LogFormat("value: {0}", value.GetValue());

            // Output:
            // value: 10
        }
    }
}
```

Or you can use the path.

```cs
using UGF.Json.Runtime;
using UnityEngine;

namespace Example.Runtime
{
    public class ExampleRead2 : MonoBehaviour
    {
        private void Start()
        {
            var textAsset = Resources.Load<TextAsset>("data_0");

            var member = JsonTextUtility.Read(textAsset.text);
            var value = member.Find("body.value");

            Debug.LogFormat("value: {0}", value.GetValue());

            // Output:
            // value: 10
        }
    }
}
```

Also you can retrieve the value directly through the path.

```cs
using UGF.Json.Runtime;
using UnityEngine;

namespace Example.Runtime
{
    public class ExampleRead3 : MonoBehaviour
    {
        private void Start()
        {
            var textAsset = Resources.Load<TextAsset>("data_0");

            var member = JsonTextUtility.Read(textAsset.text);
            double value = member.FindValue("body.value", 0D);

            Debug.LogFormat("value: {0}", value);

            // Output:
            // value: 10
        }
    }
}
```

Even through the complicated **Json** structure.

```json
{
    "body": {
        "array": [
            "value",
            {
                "body": {
                    "value": 10
                }
            }
        ]
    }
}
```
> For this example **Json** text located in **Resources** folder in `data_1.json` file.

Retrieve the value directly through the path.

```cs
using UGF.Json.Runtime;
using UnityEngine;

namespace Example.Runtime
{
    public class ExampleRead4 : MonoBehaviour
    {
        private void Start()
        {
            var textAsset = Resources.Load<TextAsset>("data_1");

            var member = JsonTextUtility.Read(textAsset.text);
            double value = member.FindValue("body.array.1.body.value", 0D);
            
            Debug.LogFormat("value: {0}", value);

            // Output:
            // value: 10
        }
    }
}
```

And you can use **JsonReader** class directly, to prevent memory allocation.

```cs
using UGF.Json.Runtime;
using UnityEngine;

namespace Example.Runtime
{
    public class ExampleRead5 : MonoBehaviour
    {
        private readonly JsonReader m_reader = new JsonReader();

        private void Start()
        {
            var textAsset = Resources.Load<TextAsset>("data_1");

            var member = m_reader.Read(textAsset.text);
            double value = member.FindValue("body.array.1.body.value", 0D);
            
            Debug.LogFormat("value: {0}", value);

            // Output:
            // value: 10
        }
    }
}
```

---
> Unity Game Framework | Alexander Vorobyov | Copyright 2018