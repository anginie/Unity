using UnityEngine;
using System.Collections;

public class Knight : MonoBehaviour {

    struct Pos
    {
        public int row;
        public int col;
        public Sprite sprite;
    }

    Pos currentPos;
    int size = 60;
    int[,] map;

	void Start () {
        map = new int[4,4];
        InitPoses();
	}

    void InitPoses()
    {
        int maxC = map.GetLength (0);
        int maxR = map.GetLength (1);
        for(int i = 0; i<maxC; i++) {
            for(int j = 0; j<maxR; j++){
                GameObject qd = GameObject.CreatePrimitive(PrimitiveType.Quad);
                SpriteRenderer sr = qd.AddComponent<SpriteRenderer>();
                Sprite sp = Sprite.Create(new Texture2D(100, 100), new Rect(), new Vector2());
                sr.sprite = sp;
                qd.transform.localScale = new Vector3(size, size, 1);
                qd.transform.localPosition = new Vector3(size * (i - 0.5f * maxC), size * (0.5f * maxR - j), 0f);
                //Sprite sp = NGUITools.AddSprite(gameObject, SLHelper.MainAtlas, "black");
                //sp.width = sp.height = size;
                //sp.transform.localPosition = new Vector3(size * (i - 0.5f * maxC), size * (0.5f * maxR - j), 0f);
                BoxCollider bc = qd.AddComponent<BoxCollider>();
                bc.size = new Vector2(1f * size, 1f * size);
                //UIEventTrigger el = sp.gameObject.AddMissingComponent<UIEventTrigger>();
                //Pos po = new Pos { row = i, col = j, sprite = sp };
                //EventDelegate.Add(el.onClick, () => OnClickPos(po));


                //if (i == 0 && j == 0)
                //{
                //    currentPos = po;
                //    map[i, j] = 1;
                //    sp.spriteName = "white";
                //    sp.alpha = 0.5f;
                //}
                //else
                //{
                //    map[i, j] = 0;
                //}
            }
        }
    }

    void OnClickPos(Pos pos)
    {
        if(map[pos.col, pos.row] == 0 &&
           ((pos.col == currentPos.col + 2 && pos.row == currentPos.row + 1) || 
           (pos.col == currentPos.col + 2 && pos.row == currentPos.row - 1) ||
           (pos.col == currentPos.col - 2 && pos.row == currentPos.row + 1) ||
           (pos.col == currentPos.col - 2 && pos.row == currentPos.row - 1) ||
           (pos.col == currentPos.col + 1 && pos.row == currentPos.row + 2) ||
           (pos.col == currentPos.col + 1 && pos.row == currentPos.row - 2) ||
           (pos.col == currentPos.col - 1 && pos.row == currentPos.row + 2) ||
           (pos.col == currentPos.col - 1 && pos.row == currentPos.row - 2))) {
            //currentPos.sprite.alpha = 1f;
            //currentPos = pos;
            //currentPos.sprite.alpha = 0.5f;
            //map[pos.col, pos.row] = 1;
            //currentPos.sprite.spriteName = "white";
        }
    }
}
