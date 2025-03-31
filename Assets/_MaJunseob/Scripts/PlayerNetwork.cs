using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    [SerializeField] private Transform spawnedObjectPrefab;

    private Transform spawnedObjectTransform;


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
            //randomNumber.Value = new MyCustomData
            //{
            //    _int = 10,
            //    _bool = false,
            //    message = "Hello World"
            //};

            //TestServerRpc("ServerRpc message");

            //TestServerRpc(new ServerRpcParams());

            //TestClientRpc();

            // 특정 클라이언트(1)에게만 호출
            //TestClientRpc(
            //    new ClientRpcParams { 
            //        Send = new ClientRpcSendParams { 
            //            TargetClientIds = new List<ulong> { 1 } } }); 

            //Instantiate(spawnedObjectPrefab);

            spawnedObjectTransform = Instantiate(spawnedObjectPrefab); 
            spawnedObjectTransform.GetComponent<NetworkObject>().Spawn(true); 
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            if(spawnedObjectTransform != null)
                Destroy(spawnedObjectTransform.gameObject);
        }


        // 플레이어 이동
        float moveSpeed = 5f;
        if (Input.GetKey(KeyCode.W)) transform.position += new Vector3(0, 0, 1) * moveSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.S)) transform.position += new Vector3(0, 0, -1) * moveSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.A)) transform.position += new Vector3(-1, 0, 0) * moveSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.D)) transform.position += new Vector3(1, 0, 0) * moveSpeed * Time.deltaTime;

    }

    /// <summary>
    /// 함수 앞에 [ServerRpc] 어트리뷰트를 붙여야 합니다.
    /// 함수 이름 뒤에 ServerRpc를 붙이는 규칙이 있습니다.
    /// ServerRpc는 서버에서만 실행되는 함수입니다.
    /// client에서 호출하면 서버에서 실행됩니다.
    /// 매개변수를 전달 할 수 있지만 refrence type은 전달할 수 없습니다.
    /// 하지만 string을 전달할 수 있는 예외가 있습니다. 
    /// FixedString128Bytes 사용하지 않아도 됩니다.
    /// </summary>
    //[ServerRpc]
    //private void TestServerRpc(string message)
    //{
    //    Debug.Log("TestServerRpc " + OwnerClientId + "; " + message);
    //}
    [ServerRpc]
    private void TestServerRpc(ServerRpcParams serverRpcParams)
    {
        Debug.Log("TestServerRpc " + OwnerClientId + "; " + serverRpcParams.Receive.SenderClientId);
    }

    /// <summary>
    /// 함수 앞에 [ClientRpc] 어트리뷰트를 붙여야 합니다.
    /// 함수 이름 뒤에 ClientRpc를 붙이는 규칙이 있습니다.
    /// server에서 호출하면 모든 클라이언트에서 실행됩니다.
    /// client는 ClientRpc를 호출할 수 없습니다.
    /// </summary>
    //[ClientRpc]
    //private void TestClientRpc()
    //{
    //    Debug.Log("TestClientRpc");
    //}

    /// <summary>
    /// IReadOnlyList<ulong> TargetClientIds을 매개변수로 전달하여 특정 클라이언트에게만 호출할 수 있습니다.
    /// server에서 특정된 하나 이상의 클라이언트로 메시지를 보낼 때 사용합니다.
    /// <summary>
    [ClientRpc]
    private void TestClientRpc(ClientRpcParams clientRpcParams)
    {
        Debug.Log("TestClientRpc");
    }

}
