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
        //public string message; // �� ������ ����ȭ���� �ʽ��ϴ�. reference type�� ����ȭ���� �ʰ� value type�� ����ȭ�˴ϴ�.
        public FixedString128Bytes message; // FixedString�� ����ȭ�˴ϴ�. 

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            /// ����Ƽ�� �⺻������ �����ϴ� ���ĵ�(int, float, Vector3, Quaternion ��)�� �ڵ����� ����ȭ�˴ϴ�.
            /// ����Ƽ Netcode������ �⺻ �ڷ��� �ܿ� ����ڰ� ���� Ŭ������ ����ü�� 
            /// ��Ʈ��ũ�� ������ ���� ���� ����ȭ�� ������ ���� �ֽ��ϴ�.
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
        
        if (!IsOwner) return; // �����ڰ� �ƴϸ� �Ʒ� �ڵ带 �������� �ʽ��ϴ�.

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

            // Ư�� Ŭ���̾�Ʈ(1)���Ը� ȣ��
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


        // �÷��̾� �̵�
        float moveSpeed = 5f;
        if (Input.GetKey(KeyCode.W)) transform.position += new Vector3(0, 0, 1) * moveSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.S)) transform.position += new Vector3(0, 0, -1) * moveSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.A)) transform.position += new Vector3(-1, 0, 0) * moveSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.D)) transform.position += new Vector3(1, 0, 0) * moveSpeed * Time.deltaTime;

    }

    /// <summary>
    /// �Լ� �տ� [ServerRpc] ��Ʈ����Ʈ�� �ٿ��� �մϴ�.
    /// �Լ� �̸� �ڿ� ServerRpc�� ���̴� ��Ģ�� �ֽ��ϴ�.
    /// ServerRpc�� ���������� ����Ǵ� �Լ��Դϴ�.
    /// client���� ȣ���ϸ� �������� ����˴ϴ�.
    /// �Ű������� ���� �� �� ������ refrence type�� ������ �� �����ϴ�.
    /// ������ string�� ������ �� �ִ� ���ܰ� �ֽ��ϴ�. 
    /// FixedString128Bytes ������� �ʾƵ� �˴ϴ�.
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
    /// �Լ� �տ� [ClientRpc] ��Ʈ����Ʈ�� �ٿ��� �մϴ�.
    /// �Լ� �̸� �ڿ� ClientRpc�� ���̴� ��Ģ�� �ֽ��ϴ�.
    /// server���� ȣ���ϸ� ��� Ŭ���̾�Ʈ���� ����˴ϴ�.
    /// client�� ClientRpc�� ȣ���� �� �����ϴ�.
    /// </summary>
    //[ClientRpc]
    //private void TestClientRpc()
    //{
    //    Debug.Log("TestClientRpc");
    //}

    /// <summary>
    /// IReadOnlyList<ulong> TargetClientIds�� �Ű������� �����Ͽ� Ư�� Ŭ���̾�Ʈ���Ը� ȣ���� �� �ֽ��ϴ�.
    /// server���� Ư���� �ϳ� �̻��� Ŭ���̾�Ʈ�� �޽����� ���� �� ����մϴ�.
    /// <summary>
    [ClientRpc]
    private void TestClientRpc(ClientRpcParams clientRpcParams)
    {
        Debug.Log("TestClientRpc");
    }

}
