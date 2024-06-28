using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : Singleton<GameAssets>
{
    protected override void InternalInit()
    {
    }

    protected override void InternalOnDestroy()
    {
    }


    public GameObject chatBubblePF;
    public List<RecipeSo> recipeSoList;
    public List<RestaurantTable> tables;




    [Header("Texture/sprities")]
    public Sprite buttonRed;
    public Sprite buttonGreen;
    public Sprite buttonGray;
}
