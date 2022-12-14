using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Inven : UI_Scene
{
    enum GameObjects
    {
        GridPanel
    }

    public override void Init()
    {
        base.Init();

        Bind<GameObject>(typeof(GameObjects));

        GameObject gridPanel = Get<GameObject>((int)GameObjects.GridPanel);

        // 그리드 안에 있는 자식을 모두 삭제
        foreach(Transform child in gridPanel.transform){
            Managers.Resource.Destroy(child.gameObject);
        }

        // 실제 인벤토리 정보를 참고해서 자식을 다시 채움
        for(int i=0; i<8; i++){
            GameObject item = Managers.UI.MakeSubItem<UI_Inven_Item>(parent: gridPanel.transform).gameObject;

            UI_Inven_Item invenItem = item.GetOrAddComponent<UI_Inven_Item>();
            invenItem.SetInfo($"집행검{i}");
        }
    }
}
