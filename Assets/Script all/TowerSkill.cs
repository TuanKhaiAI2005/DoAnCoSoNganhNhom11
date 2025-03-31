using UnityEngine;
using System.Collections;
using TMPro; // Thêm thư viện TextMeshPro
using UnityEngine.UI; // Thêm thư viện UI để sử dụng Slider

public class TowerSkill : MonoBehaviour
{
    public float skillCooldown = 30f; // Thời gian hồi chiêu
    private float nextSkillTime = 0f;
    public GameObject explosionEffect; // Hiệu ứng nổ
    public float skillDamage = 50f; // Sát thương kỹ năng
    public float skillRange = 5f; // Tầm bắn kỹ năng
    public AudioSource skillSound; // Âm thanh kỹ năng

    public TextMeshPro cooldownText; // UI hiển thị thời gian hồi chiêu
    public Slider cooldownSlider; // Thanh hồi chiêu

    void Start()
    {
        if (cooldownText != null)
        {
            cooldownText.text = "Sẵn sàng";
        }

        if (cooldownSlider != null)
        {
            cooldownSlider.maxValue = skillCooldown;
            cooldownSlider.value = 0; // Ban đầu set về 0
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && Time.time >= nextSkillTime)
        {
            ActivateSkill();
            nextSkillTime = Time.time + skillCooldown;

            if (cooldownSlider != null)
            {
                cooldownSlider.value = cooldownSlider.maxValue; // Set thanh đầy
            }
        }

        UpdateCooldownUI();
    }

    void ActivateSkill()
    {
        if (skillSound != null)
        {
            skillSound.Play(); // Phát âm thanh kỹ năng
        }
        
        GameObject closestEnemy = FindClosestEnemy();
        if (closestEnemy != null)
        {
            Vector3 explosionPosition = closestEnemy.transform.position;
            GameObject explosion = Instantiate(explosionEffect, explosionPosition, Quaternion.identity);
            
            if (explosion.GetComponent<ParticleSystem>() != null)
            {
                Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.duration);
            }
            else if (explosion.GetComponent<Animator>() != null)
            {
                StartCoroutine(DestroyAfterAnimation(explosion));
            }

            EnemyStats enemyStats = closestEnemy.GetComponent<EnemyStats>();
            if (enemyStats != null)
            {
                enemyStats.TakeDamage((int)skillDamage);
                Debug.Log("💥 Kỹ năng kích hoạt! Gây sát thương: " + skillDamage);
            }
            else
            {
                Debug.LogWarning("❌ Không tìm thấy EnemyStats trên quái!");
            }

            StartCoroutine(ScreenShake(0.2f, 0.3f));
        }
        else
        {
            Debug.Log("❌ Không có quái nào trong phạm vi!");
        }
    }

    void UpdateCooldownUI()
    {
        float timeLeft = nextSkillTime - Time.time;

        // Cập nhật Text hiển thị thời gian hồi chiêu
        if (cooldownText != null)
        {
            if (timeLeft > 0)
            {
                cooldownText.text = Mathf.Ceil(timeLeft).ToString() + "s";
            }
            else
            {
                cooldownText.text = "Sẵn sàng";
            }

            cooldownText.transform.position = transform.position + new Vector3(0, -1f, 0);
        }

        // Cập nhật thanh hồi chiêu
        if (cooldownSlider != null)
        {
            cooldownSlider.value = skillCooldown - timeLeft; 

            if (timeLeft <= 0)
            {
                cooldownSlider.value = 0; // Khi hồi chiêu xong, set về 0
            }

            cooldownSlider.transform.position = transform.position + new Vector3(0, -1f, 0); 
        }
    }

    GameObject FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float minDistance = skillRange;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = enemy;
            }
        }

        return closest;
    }

    IEnumerator DestroyAfterAnimation(GameObject effect)
    {
        Animator anim = effect.GetComponent<Animator>();
        if (anim != null)
        {
            yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        }
        Destroy(effect);
    }

    IEnumerator ScreenShake(float duration, float magnitude)
    {
        Vector3 originalPos = Camera.main.transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            Camera.main.transform.position = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);
            elapsed += Time.deltaTime;
            yield return null;
        }

        Camera.main.transform.position = originalPos;
    }
}
