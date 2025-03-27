using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{

    private NetworkVariable<MyCustomData> randomNumber = new NetworkVariable<MyCustomData>(
            new MyCustomData { 
                _int = 56, 
                _bool = true,
            }, 
            NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


    public struct MyCustomData : INetworkSerializable
    {
        public int _int;
        public bool _bool;
        //public string message; // 이 변수는 직렬화되지 않습니다. reference type은 직렬화되지 않고 value type만 직렬화됩니다.
        public FixedString128Bytes message; // FixedString은 직렬화됩니다. 

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            /// 유니티가 기본적으로 지원하는 형식들(int, float, Vector3, Quaternion 등)은 자동으로 직렬화됩니다.
            /// 유니티 Netcode에서는 기본 자료형 외에 사용자가 만든 클래스나 구조체를 
            /// 네트워크로 전송할 때는 직접 직렬화를 구현할 수도 있습니다.
            serializer.SerializeValue(ref _int);
            serializer.SerializeValue(ref _bool);
            serializer.SerializeValue(ref message);
        }
    }

    public override void OnNetworkSpawn()
    {
        randomNumber.OnValueChanged += (MyCustomData previousValue, MyCustomData newValue) => {
            Debug.Log(OwnerClientId + "; " + newValue._int + "; " + newValue._bool + "; " + newValue.message);
        };

    }

    private void Update()
    {
        //Debug.Log(OwnerClientId + ";  randomNumber: " + randomNumber.Value);

        
        if (!IsOwner) return; // 소유자가 아니면 아래 코드를 실행하지 않습니다.

        if (Input.GetKeyDown(KeyCode.T))
        {
            randomNumber.Value = new MyCustomData
            {
                _int = 10,
                _bool = false,
                message = "Hello World"
            };
        }

        // 플레이어 이동
        float moveSpeed = 5f;

        if (Input.GetKey(KeyCode.W))
        {
            transform.position += new Vector3(0, 0, 1) * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += new Vector3(0, 0, -1) * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += new Vector3(-1, 0, 0) * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(1, 0, 0) * moveSpeed * Time.deltaTime;
        }

    }


}
