using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VirtualDPad : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private Image imageBackground;  // 우리가 실제 터치하는 컨트롤러의 배경 이미지
    [SerializeField]
    private Image[] images;  
    private Vector2 touchPosition;

    private Color pushedColor = Color.yellow;


    public float Horizontal { get { return touchPosition.x; } }
    public float Vertical { get { return touchPosition.y; } }

    private void Awake()
    {
        imageBackground = this.GetComponent<Image>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnDrag(PointerEventData eventData)
    {
        touchPosition = Vector2.zero;


        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            imageBackground.rectTransform, eventData.position, eventData.pressEventCamera, out touchPosition))
        {

            touchPosition.x = (touchPosition.x / imageBackground.rectTransform.sizeDelta.x);
            touchPosition.y = (touchPosition.y / imageBackground.rectTransform.sizeDelta.y);


            touchPosition = new Vector2(touchPosition.x * 2 - 1, touchPosition.y * 2 - 1);

            touchPosition =  touchPosition.normalized ;

            ChangePadColor();


        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {

        touchPosition = Vector2.zero;
        ResetPadColor();
    }

    private void ResetPadColor()
    {
        foreach(Image arrow in images)
        {
            arrow.color = Color.white;
        }
    }
    private void ChangePadColor()
    {
        ResetPadColor();

        if(touchPosition.x > 0.2f || touchPosition.x < -0.2f || touchPosition.y > 0.2f || touchPosition.y < -0.2f)
        {
            if (touchPosition.x > 0)
            {
                images[1].color = pushedColor;
            }
            else if (touchPosition.x < 0)
            {
                images[2].color = pushedColor;
            }
            if (touchPosition.y > 0)
            {
                images[0].color = pushedColor;
            }
            else if (touchPosition.y < 0)
            {
                images[3].color = pushedColor;
            }
        }
        else
        {
            if (touchPosition.x <0 && touchPosition.x >= -2)
            {
                images[1].color = pushedColor;
            }
            else if (touchPosition.x <= 2 && touchPosition.x > 0)
            {
                images[1].color = pushedColor;
            }
            else
            if (touchPosition.y <0 && touchPosition.y >= -2)
            {
                images[1].color = pushedColor;
            }
            else
            if (touchPosition.y <=2 && touchPosition.y > 0)
            {
                images[1].color = pushedColor;
            }

        }
               
        

    }
}

