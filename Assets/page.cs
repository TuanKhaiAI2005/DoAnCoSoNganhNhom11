using UnityEngine;
using UnityEngine.UI;

public class SlideManager : MonoBehaviour
{
    public GameObject[] slides; // Gán tất cả các panel vào đây theo thứ tự
    public Button nextButton;   // Nút chuyển slide tiếp theo
    public Button prevButton;   // Nút quay lại slide trước
    private int currentIndex = 0;

    void Start()
    {
        // Hiển thị trang đầu tiên, ẩn tất cả các trang còn lại
        for (int i = 0; i < slides.Length; i++)
        {
            slides[i].SetActive(i == 0);
        }

        // Gán sự kiện cho nút
        nextButton.onClick.AddListener(NextSlide);
        prevButton.onClick.AddListener(PrevSlide);

        // Kiểm tra trạng thái nút
        UpdateButtonStates();
    }

    void NextSlide()
    {
        if (currentIndex < slides.Length - 1)
        {
            slides[currentIndex].SetActive(false);
            currentIndex++;
            slides[currentIndex].SetActive(true);
            UpdateButtonStates();
        }
    }

    void PrevSlide()
    {
        if (currentIndex > 0)
        {
            slides[currentIndex].SetActive(false);
            currentIndex--;
            slides[currentIndex].SetActive(true);
            UpdateButtonStates();
        }
    }

    void UpdateButtonStates()
    {
        prevButton.interactable = currentIndex > 0;
        nextButton.interactable = currentIndex < slides.Length - 1;
    }
}
