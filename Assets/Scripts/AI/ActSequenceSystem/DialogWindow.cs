using TMPro;
using UnityEngine;
using UnityEngine.UI;

//namespace ActSequenceSystem
//{
//    public class DialogWindow : MonoBehaviour
//    {
//        public TMP_Text dialogText;

//        public ActSequenceGraph activeDialog;

//        public GameObject buttonPrefab;

//        public Transform buttonParent;

//        private int segmentIndex = 0;

//        private GoToPoint activeSegment;

//        void Start()
//        {
//            // Find Start Node
//            foreach (GoToPoint node in activeDialog.nodes)
//            {
//                if (!node.GetInputPort("input").IsConnected)
//                {
//                    UpdateDialog(node);
//                    break;
//                }
//            }
//        }

//        public void AnswerClicked(int clickedIndex)
//        {
//            var port = activeSegment.GetPort("Answers " + clickedIndex);
//            //var port = activeSegment.GetOutputPort()
//            if (port.IsConnected)
//                UpdateDialog(port.Connection.node as GoToPoint);
//            else
//                gameObject.SetActive(false);
//        }

//        private void UpdateDialog(GoToPoint newSegment)
//        {
//            activeSegment = newSegment;
//            //dialogText.text = newSegment.DialogText;
//            int answerIndex = 0;
//            foreach (Transform child in buttonParent)
//            {
//                Destroy(child.gameObject);
//            }

//            foreach (var answer in newSegment.output)
//            {
//                var btn = Instantiate(buttonPrefab, buttonParent);
//                btn.GetComponentInChildren<Text>().text = answer;

//                var index = answerIndex;
//                btn.GetComponentInChildren<Button>().onClick.AddListener((() => { AnswerClicked(index); }));

//                answerIndex++;
//            }
//        }
//    }
//}