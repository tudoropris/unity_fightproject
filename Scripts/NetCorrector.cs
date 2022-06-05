using UnityEngine;
using UnityEngine.Networking;
using System;

public class NetCorrector : NetworkBehaviour
{

    [SyncVar(hook = "FacingCallback")]
    public bool netFacingRight = true;

    [Command]
     public void CmdFlipSprite(bool facing)
    {
        netFacingRight = facing;
        if (netFacingRight)
        {
            Vector3 SpriteScale = transform.localScale;
            SpriteScale.x = 1;
            transform.localScale = SpriteScale;
        }
        else
                     {
            Vector3 SpriteScale = transform.localScale;
            SpriteScale.x = -1;
            transform.localScale = SpriteScale;
        }
             }

    void FacingCallback(bool facing)
    {
        netFacingRight = facing;
                 if (netFacingRight)
        {
            Vector3 SpriteScale = transform.localScale;
            SpriteScale.x = 1;
            transform.localScale = SpriteScale;
                    }
        else
        {
            Vector3 SpriteScale = transform.localScale;
            SpriteScale.x = -1;
            transform.localScale = SpriteScale;
        }
    }
}