Netcode for GameObjects

참고 자료
https://www.youtube.com/watch?v=3yuBOB3VrCk
https://docs-multiplayer.unity3d.com/netcode/current/installation/

ParrelSync
https://github.com/VeriorPies/ParrelSync

유튜브 영상 자료에 나오지 않는 문제점:
NetworkAnimator 챕터에서 Client 캐릭터가 움직이지 않는다. 
플레이어 캐릭터의 PlayerInput 컴포넌트를 끈 상태를 기본으로 하고 아래의 코드를 추가한다. 

void Start()
{
            //...
            
            _playerInput = GetComponent<PlayerInput>();
            if(IsOwner) _playerInput.enabled = true;
            
            //...
}


https://docs-multiplayer.unity3d.com/netcode/current/components/networkanimator/#owner-authoritative-mode

using Unity.Netcode.Components;
public class OwnerNetworkAnimator : NetworkAnimator
{
    protected override bool OnIsServerAuthoritative()
    {
        return false;
    }
}


[Create a game with a listen server and host architecture](https://docs-multiplayer.unity3d.com/netcode/current/learn/listen-server-host-architecture/)



다음으로 학습할 내용：
Making a MULTIPLAYER Game? Join your Players with LOBBY!
https://www.youtube.com/watch?v=-KDlEBfCBiU

How to use Unity Relay, Multiplayer through FIREWALL! (Unity Gaming Services)
https://www.youtube.com/watch?v=msPNJ2cxWfw

How to make Simple Multiplayer Game! (FREE Course Unity Tutorial Netcode for Game Objects)
https://www.youtube.com/watch?v=YmUnXsOp_t0

Learn Netcode for Entities (FREE Course, SUPER FAST Multiplayer with Unity DOTS)
https://www.youtube.com/watch?v=-3EVvJ8kr1U

https://unity.com/products/netcode

Educational samples: Boss Room
https://unity.com/demos/small-scale-coop-sample
https://docs-multiplayer.unity3d.com/netcode/current/learn/bossroom/bossroom/

Learn Unity Multiplayer (FREE Complete Course, Netcode for Game Objects Unity Tutorial 2024)
https://www.youtube.com/watch?v=7glCsF9fv3s&list=PLzDRvYVwl53sSmEcIgZyDzrc0Smpq_9fN&index=1
