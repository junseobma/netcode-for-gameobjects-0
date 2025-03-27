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
            randomNumber.Value = new MyCustomData
            {
                _int = 10,
                _bool = false,
                message = "Hello World"
            };
        }

        // �÷��̾� �̵�
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
