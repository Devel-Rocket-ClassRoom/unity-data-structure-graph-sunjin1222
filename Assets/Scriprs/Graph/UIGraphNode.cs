
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGraphNode : MonoBehaviour
{
    public Image Image;

    public TextMeshProUGUI text;
    private GraphNode node;


    public void Reset2()
    {
        setcolor(node.Canvisit ? Color.white : Color.gray);
        setText($"Id:{node.id}\nweight:{node.weight}");
    }

    public void setNode(GraphNode node)
    {
        this.node = node;
    }
    public void setcolor(Color color)
    {
        Image.color=(color);
    }
    public void setText(string txst)
    {
        this.text.text= txst;
    }



}
