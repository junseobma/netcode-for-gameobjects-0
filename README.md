Netcode for GameObjects

참고 자료
https://www.youtube.com/watch?v=3yuBOB3VrCk

ParrelSync
https://github.com/VeriorPies/ParrelSync

영상에 나오지 않는 문제점:
NetworkAnimator 챕터에서 Client 캐릭터가 움직이지 않는다. 
플레이어 캐릭터의 PlayerInput 컴포넌트를 끈 상태를 기본으로 하고 아래의 코드를 추가한다. 

void Start()
{
...
_playerInput = GetComponent<PlayerInput>();
            if(IsOwner)
                _playerInput.enabled = true;
}

다음으로 학습할 내용：
Making a MULTIPLAYER Game? Join your Players with LOBBY!
https://www.youtube.com/watch?v=-KDlEBfCBiU

How to use Unity Relay, Multiplayer through FIREWALL! (Unity Gaming Services)
https://www.youtube.com/watch?v=msPNJ2cxWfw

How to make Simple Multiplayer Game! (FREE Course Unity Tutorial Netcode for Game Objects)
https://www.youtube.com/watch?v=YmUnXsOp_t0

Learn Netcode for Entities (FREE Course, SUPER FAST Multiplayer with Unity DOTS)
https://www.youtube.com/watch?v=-3EVvJ8kr1U
