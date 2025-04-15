# Netcode for GameObjects

### 참고 자료
* [***COMPLETE Unity Multiplayer Tutorial (Netcode for Game Objects)***](https://www.youtube.com/watch?v=3yuBOB3VrCk)  
* [Install Netcode for GameObjects](https://docs-multiplayer.unity3d.com/netcode/current/installation/)  
* [Build multiplayer games with Unity Netcode](https://unity.com/products/netcode)  

### ParrelSync
https://github.com/VeriorPies/ParrelSync

### 구현
[PlayerNetwork.cs](https://github.com/junseobma/netcode-for-gameobjects-0/blob/main/Assets/_MaJunseob/Scripts/PlayerNetwork.cs)

### 유튜브 참고 자료의 문제점 및 해결법
NetworkAnimator 챕터에서 Client 캐릭터가 움직이지 않는다.  
플레이어 캐릭터 프리팹의 PlayerInput 컴포넌트를 끄고 아래의 코드를 추가한다.
```C#
void Start()
{
            //...
            
            _playerInput = GetComponent<PlayerInput>();
            if(IsOwner) _playerInput.enabled = true;
}
```

[Owner authoritative mode](https://docs-multiplayer.unity3d.com/netcode/current/components/networkanimator/#owner-authoritative-mode)
```C#
using Unity.Netcode.Components;
public class OwnerNetworkAnimator : NetworkAnimator
{
    protected override bool OnIsServerAuthoritative()
    {
        return false;
    }
}
```

[Create a game with a listen server and host architecture](https://docs-multiplayer.unity3d.com/netcode/current/learn/listen-server-host-architecture/)

---------------------------------------------------
### 다음으로 학습할 내용
**Unity Code Monkey**  
* [Making a MULTIPLAYER Game? Join your Players with LOBBY!](https://www.youtube.com/watch?v=-KDlEBfCBiU)  
* [How to use Unity Relay, Multiplayer through FIREWALL! (Unity Gaming Services)](https://www.youtube.com/watch?v=msPNJ2cxWfw)  
* [How to make Simple Multiplayer Game! (FREE Course Unity Tutorial Netcode for Game Objects)](https://www.youtube.com/watch?v=YmUnXsOp_t0)  
* [Learn Netcode for Entities (FREE Course, SUPER FAST Multiplayer with Unity DOTS)](https://www.youtube.com/watch?v=-3EVvJ8kr1U)  
* [Learn Unity Multiplayer (FREE Complete Course, Netcode for Game Objects Unity Tutorial 2024)](https://www.youtube.com/watch?v=7glCsF9fv3s&list=PLzDRvYVwl53sSmEcIgZyDzrc0Smpq_9fN&index=1)  

**Educational samples: Boss Room**  
* https://unity.com/demos/small-scale-coop-sample  
* https://docs-multiplayer.unity3d.com/netcode/current/learn/bossroom/bossroom/  
