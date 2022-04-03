using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Battle
{
    public class PartyMemberUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI hpText;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private Image hpBar;
        [SerializeField] private Image icon;

        private Pokemon pokemon;
        private Image image;

        private void Awake()
        {
            image = GetComponent<Image>();
        }
        private float NormalizedHp => (float)pokemon.HP / pokemon.MaxHP;
        public void SetData(Pokemon pokemon)
        {
            this.pokemon = pokemon;

            icon.sprite = pokemon.Base.Sprite.Box;
            nameText.text = pokemon.Name;
            hpText.text = $"{pokemon.HP}/{pokemon.MaxHP}";
            levelText.text = $"Lv. {pokemon.Level}";
            hpBar.transform.localScale = new Vector3(NormalizedHp, 1);
        }

        public void SetSelected()
        {
            image.color = Color.black;
                nameText.color = Color.white;
                hpText.color = Color.white;
                levelText.color = Color.white;
        }
        public void SetUnselected()
        {
            image.color = Color.white;
            nameText.color = Color.black;
            hpText.color = Color.black;
            levelText.color = Color.black;
        }
    }
}
