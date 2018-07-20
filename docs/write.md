[< Back](../readme.md)

## Write

Example of how to build **Json** data and how to convert it to **Json** structure.

```cs
using UGF.Json.Runtime;
using UGF.Json.Runtime.Members;
using UnityEngine;

namespace Example.Runtime
{
    public class ExampleWrite1 : MonoBehaviour
    {
        private void Start()
        {
            var member = new JsonObject();

            member.Add("value", new JsonValue<double>(10D));

            string text = JsonTextUtility.Write(member);

            Debug.Log(text);

            // Output:
            // {"value":10}
        }
    }
}
```

And the complicated **Json** structure.

```cs
using UGF.Json.Runtime;
using UGF.Json.Runtime.Members;
using UnityEngine;

namespace Example.Runtime
{
    public class ExampleWrite2 : MonoBehaviour
    {
        private void Start()
        {
            var member = new JsonObject();
            var array = new JsonArray();
            
            member.Add("array", array);

            array.Add(new JsonValue<string>("This is a text."));
            array.Add(new JsonValue<string>("This is a second text."));

            var body = new JsonObject();

            body.Add("bool", new JsonValue<bool>(true));
            body.Add("float", new JsonValue<float>(12F));

            array.Add(body);
            
            string text = JsonTextUtility.Write(member);

            Debug.Log(text);

            // Output:
            // {"array":["This is a text.","This is a second text.",{"bool":true,"float":12}]}
        }
    }
}
```

Circular references not allowed and will be skipped.

```cs
using UGF.Json.Runtime;
using UGF.Json.Runtime.Members;
using UnityEngine;

namespace Example.Runtime
{
    public class ExampleWrite3 : MonoBehaviour
    {
        private void Start()
        {
            var member = new JsonObject();

            member.Add("circular", member);

            string text = JsonTextUtility.Write(member);
            
            Debug.Log(text);
            
            // Output:
            // Member circular references detected, member will be skipped: key 'circular'.
            // {}
        }
    }
}
```

And you can use **JsonWriter** class directly, to prevent memory allocation.

```cs
using UGF.Json.Runtime;
using UGF.Json.Runtime.Members;
using UnityEngine;

namespace Example.Runtime
{
    public class ExampleWrite4 : MonoBehaviour
    {
        private readonly JsonWriter m_writer = new JsonWriter();

        private void Start()
        {
            var member = new JsonObject();

            member.Add("value", new JsonValue<double>(10D));

            string text = m_writer.Write(member);

            Debug.Log(text);

            // Output:
            // {"value":10}
        }
    }
}
```

---
> Unity Game Framework | Alexander Vorobyov | Copyright 2018