using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // HGJT

// CircleAvartar에 적용할 스크립트
public class CircleAvartar : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Image image; // HGJT
    public Text logtext;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        image = GetComponent<Image>(); // HGJT
    }

    // 프로필 이미지를 업데이트 하는 함수
    public void updateProfileImage() {
        Debug.Log("updateProfileImage 함수 실행");
        // 갤러리에서 Texture2D로 이미지 가져오기
        //Texture2D texture = loadProfile(1);
        Texture2D texture = loadProfile(1);

        // Texture2D -> Sprite로 변환
        //Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        //spriteRenderer.sprite = sprite;
        image.sprite = sprite; // HGJT

       
    }

    // 갤러리에서 이미지를 선택하는 함수
    public Texture2D loadProfile(int size) {
        Texture2D texture = null;
        // 갤러리에서 이미지 검색
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) => {
            Debug.Log("이미지 경로: " + path);
            logtext.text = path.ToString();
            //string logtext = path;


            
            // 이미지 경로를 찾았다면
            if (path != null) {
                // 해당 이미지 Texture화
                texture = NativeGallery.LoadImageAtPath(path, size);
                if (texture == null) {
                    Debug.Log("텍스쳐를 로드하지 못했습니다. 경로: " + path);
                }
            }
        });

        if (texture == null) {
            Debug.Log("프로필 사진을 불러오지 못했습니다.");
        }
        return texture;
    }
    
}
