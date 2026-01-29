# Spiritbound Frontier – Mini GDD

## Khẩu hiệu (Tagline)
Xây dựng. Trồng trọt. Chiến đấu — Cùng Linh Thú Đồng Hành.

## 1. Tổng quan Game
**Thể loại:** 2D Top-down | Life-sim | Trồng trọt | Xây dựng | Chiến đấu bằng Pet | Co-op Online  
**Nền tảng:** PC (Windows) – mở rộng sau  
**Đối tượng:** Người chơi casual, fan game indie như Stardew Valley, Rune Factory

### Ý tưởng cốt lõi
Người chơi khai hoang vùng đất mới cùng Linh Thú (Pet). Lối sống hằng ngày của người chơi trực tiếp quyết định Linh Thú tiến hóa theo hướng nào.  
**Linh Thú không có class cố định — Linh Thú là hiện thân của cách bạn chơi game.**

## 2. Vòng lặp gameplay chính (Một ngày trong game)
### 🌅 Buổi sáng – Trồng trọt & chăm sóc
- Trồng cây, tưới nước, thu hoạch
- Chăm sóc Linh Thú
- Linh Thú hỗ trợ farm (buff hoặc làm tự động nhẹ)

### 🌤️ Buổi chiều – Xây dựng & quản lý
- Xây / nâng cấp nhà
- Chế tạo (craft)
- Mua – bán tại cửa hàng
- Linh Thú giúp xây nhanh hoặc giảm nguyên liệu

### 🌙 Buổi tối – Khám phá & chiến đấu
- Ra vùng hoang dã
- Linh Thú chiến đấu với quái vật
- Nhặt vật phẩm hiếm, làm nhiệm vụ

### 🔁 Tiến triển
Hành vi trong ngày → cộng điểm Lối Sống cho Linh Thú  
Linh Thú mở kỹ năng mới / thay đổi ngoại hình  
Người chơi mạnh hơn → bắt đầu ngày mới

## 3. Hệ thống Linh Thú (Pet System)
### 3.1 Chỉ số Lối Sống
Linh Thú tiến hóa dựa trên hành động của người chơi, không dựa vào level truyền thống.

| Chỉ số | Tăng khi |
| --- | --- |
| 🌱 Thiên nhiên | Trồng trọt, thu hoạch |
| ⚔️ Chiến đấu | Đánh quái, khám phá |
| 🏗️ Xây dựng | Xây nhà, chế tạo |

## 4. Cây tiến hóa Linh Thú (Pet Evolution Tree)
### 4.1 Nguyên tắc tiến hóa
- Linh Thú có 3 thanh chỉ số: Thiên nhiên / Chiến đấu / Xây dựng
- Cuối mỗi ngày, chỉ số cao nhất ảnh hưởng hình dạng Linh Thú
- Mỗi lần tiến hóa thay đổi: ngoại hình + kỹ năng + vai trò

### 4.2 Các nhánh tiến hóa
**Linh Thú Cơ Bản (Base Spirit)**
- Ngoại hình trung tính
- Kỹ năng hỗ trợ cơ bản

**⬇️ Nhánh cấp 1 (theo lối sống chính)**
- 🌱 **Linh Thú Thiên Nhiên**
  - Vai trò: Hỗ trợ / Hồi máu
  - Kỹ năng: Hồi máu nhẹ, tăng sản lượng cây trồng
  - Nội tại: Buff khi ở nông trại
- ⚔️ **Linh Thú Chiến Binh**
  - Vai trò: DPS / Tank
  - Kỹ năng: Lao tới, khiêu khích quái
  - Nội tại: Tăng sát thương
- 🏗️ **Linh Thú Công Trình**
  - Vai trò: Hỗ trợ / Khống chế
  - Kỹ năng: Tạo khiên, dựng trụ hỗ trợ
  - Nội tại: Giảm chi phí xây dựng

**⬇️ Nhánh lai (Giữa – Cuối game)**
- 🌱 + ⚔️ → **Linh Thú Druid** (Hồi máu + sát thương)
- ⚔️ + 🏗️ → **Linh Thú Hộ Vệ** (Tank + bảo vệ đồng đội)
- 🌱 + 🏗️ → **Linh Thú Kiến Trúc** (Buff farm + build mạnh)

### 4.3 Kỹ năng Linh Thú
- 2 kỹ năng chủ động
- 1 kỹ năng nội tại
- Kỹ năng mở dần theo tiến hóa, không reset để giữ cảm giác gắn bó

## 5. Hệ thống Trồng trọt (Farming)
- Farm theo ô (tile-based)
- Trạng thái đất: Cỏ → Đất → Gieo hạt → Phát triển → Thu hoạch
- Cây trồng có:
  - Thời gian sinh trưởng
  - Chất lượng (Thường / Tốt / Hiếm)

**Linh Thú Thiên Nhiên giúp:**
- Tưới nước tự động
- Tăng sản lượng
- Giảm thời gian sinh trưởng

## 6. Hệ thống Xây dựng (Building)
**Công trình:**
- Nhà chính
- Kho chứa
- Xưởng chế tạo
- Trang trí

Mỗi công trình có cấp độ → mở gameplay mới

**Linh Thú Công Trình giúp:**
- Giảm nguyên liệu
- Tăng tốc độ xây
- Mở bản vẽ đặc biệt

## 7. Hệ thống Chiến đấu (Pet là trung tâm)
**Người chơi:**
- Không đánh chính
- Né tránh, ra lệnh Linh Thú, dùng vật phẩm

**Linh Thú:**
- Tự động tấn công
- Kỹ năng có hồi chiêu
- AI đơn giản

**Quái vật:**
- Theo từng khu vực
- Rơi vật phẩm hiếm

## 8. Cửa hàng & Kinh tế
**Cửa hàng**
- Mua: hạt giống, bản vẽ, potion
- Bán: nông sản, vật phẩm rơi ra, đồ chế tạo

**Vòng lặp kinh tế**
Trồng → Bán → Mua → Xây → Mạnh hơn

Co-op online cho phép trao đổi vật phẩm giới hạn.

## 9. Co-op Online
- 2–4 người / phòng
- Vào phòng bằng mã
- Cùng trồng trọt, xây dựng, chiến đấu
- Mỗi người có Linh Thú khác vai trò → teamwork
- Hiệu ứng cộng hưởng Linh Thú
  - Linh Thú đứng gần nhau → kích hoạt combo
  - Linh Thú cùng hệ → buff đặc biệt

## 10. Phạm vi MVP (Bản đầu)
**✅ Bao gồm:**
1. 1 bản đồ nông trại
2. 1 khu vực chiến đấu
3. 3 Linh Thú cơ bản
4. Co-op 2 người
5. 1 cửa hàng

**❌ Chưa làm:**
- MMO
- PvP
- Quá nhiều Linh Thú

## 11. Bản nháp trang Steam
**Mô tả ngắn**  
Spiritbound Frontier là game life-sim co-op nhẹ nhàng, nơi lối sống của bạn quyết định Linh Thú đồng hành. Trồng trọt, xây dựng, khám phá vùng đất hoang và chiến đấu cùng bạn bè.

**Tính năng chính**
- 🌱 Trồng trọt, xây dựng, mô phỏng đời sống
- 🐾 Linh Thú tiến hóa theo cách chơi
- ⚔️ Chiến đấu dựa trên Linh Thú
- 🏡 Xây dựng căn cứ
- 🤝 Co-op online 2–4 người

**Trụ cột thiết kế**
- Lối sống quyết định Linh Thú
- Linh Thú là bạn đồng hành, không phải công cụ
- Mỗi ngày chơi đều có tiến triển rõ ràng

**Thẻ Steam (đề xuất)**
Life Sim, Farming Sim, Creature Collector, Co-op, Indie, Casual, Building, Adventure

## 12. Cây Tiến Hóa Linh Thú (Chi Tiết)
### 12.1 Tổng quan cây tiến hóa
```
                [ Linh Thú Cơ Bản ]
                         |
        ---------------------------------------
        |                  |                  |
 [Thiên Nhiên]        [Chiến Binh]        [Công Trình]
        |                  |                  |
   (🌱 Nature)        (⚔️ Battle)        (🏗️ Build)
        |                  |                  |
   ----------------   ----------------   ----------------
   |              |   |              |   |              |
[Druid]       [Bloom] [Berserker] [Guardian] [Architect] [Sentinel]
 (🌱⚔️)        (🌱🌱)    (⚔️⚔️)      (⚔️🏗️)      (🌱🏗️)      (🏗️🏗️)
```

### 12.2 Các dạng Linh Thú chi tiết
**🌱 Linh Thú Thiên Nhiên (Nature Beast)**
- Vai trò: Support / Healer
- Ngoại hình: Lá cây, hoa, ánh sáng xanh
- Kỹ năng:
  - Hồi máu theo thời gian
  - Tăng sản lượng cây trồng
- Phù hợp: Người chơi thích farm, chơi thư giãn

**⚔️ Linh Thú Chiến Binh (Battle Beast)**
- Vai trò: DPS / Tank
- Ngoại hình: Giáp, vuốt, nanh
- Kỹ năng:
  - Lao vào kẻ địch
  - Khiêu khích quái vật
- Phù hợp: Người chơi thích khám phá, đánh quái

**🏗️ Linh Thú Công Trình (Build Beast)**
- Vai trò: Utility / Control
- Ngoại hình: Đá, gỗ, rune cổ
- Kỹ năng:
  - Tạo khiên bảo vệ
  - Dựng trụ hỗ trợ
- Phù hợp: Người chơi thích xây dựng, quản lý

### 12.3 Linh Thú Lai (Hybrid)
- 🌱 + ⚔️ **Linh Thú Druid**
  - Hồi máu + sát thương phép
  - Buff khi ở rừng hoặc nông trại
- ⚔️ + 🏗️ **Linh Thú Hộ Vệ (Guardian)**
  - Tank mạnh, bảo vệ đồng đội
  - Tạo lá chắn khu vực
- 🌱 + 🏗️ **Linh Thú Kiến Trúc (Architect Spirit)**
  - Buff farm + build
  - Giảm chi phí & thời gian xây dựng

### 12.4 Quy tắc tiến hóa
- Tiến hóa diễn ra sau mỗi 7 ngày trong game hoặc khi đủ điểm
- Người chơi có thể định hướng nhưng không chọn trực tiếp
- Quyết định tiến hóa không thể quay lại (tăng giá trị lựa chọn)

## 13. Roadmap Làm MVP – 30 Ngày
### Tuần 1 – Nền tảng & Core Loop
- Ngày 1–2: Setup project Unity 6 LTS (2D URP)
- Ngày 3–4: Player movement + camera
- Ngày 5–6: Tilemap farm (đất, gieo hạt, thu hoạch)
- Ngày 7: Vòng ngày–đêm + lưu trạng thái đơn giản

**Kết quả:** Chơi được 1 ngày farm cơ bản

### Tuần 2 – Linh Thú & Tiến hóa
- Ngày 8–9: Pet follow player + animation
- Ngày 10–11: Lifestyle Points (Nature / Battle / Build)
- Ngày 12–13: Pet tiến hóa Tier 1 + đổi sprite
- Ngày 14: Skill cơ bản cho từng dạng pet

**Kết quả:** Pet thay đổi theo lối chơi

### Tuần 3 – Combat & Building
- Ngày 15–16: Khu vực đánh quái + AI quái đơn giản
- Ngày 17–18: Pet combat (auto attack + skill)
- Ngày 19–20: Hệ xây dựng (nhà, kho)
- Ngày 21: Shop mua – bán

**Kết quả:** Loop Farm → Build → Fight hoàn chỉnh

### Tuần 4 – Online & Polish
- Ngày 22–23: Netcode for GameObjects (spawn player)
- Ngày 24–25: Đồng bộ pet + farm
- Ngày 26: Kết nối Relay (vào phòng bằng mã)
- Ngày 27–28: Fix bug, cân bằng
- Ngày 29–30: Build demo + ghi video

**Kết quả:** MVP co-op 2 người có thể chơi

## 14. Cách cộng điểm (ví dụ cụ thể)
### 🌱 Trồng trọt
| Hành động | Điểm |
| --- | --- |
| Gieo hạt | +1 Nature |
| Tưới nước | +1 Nature |
| Thu hoạch | +2 Nature |
| Thu hoạch cây hiếm | +3 Nature |

### ⚔️ Chiến đấu
| Hành động | Điểm |
| --- | --- |
| Đánh quái thường | +2 Battle |
| Giết boss | +5 Battle |
| Clear 1 khu | +3 Battle |

### 🏗️ Xây dựng
| Hành động | Điểm |
| --- | --- |
| Đặt block | +1 Build |
| Hoàn thành công trình | +3 Build |
| Nâng cấp nhà | +5 Build |

👉 Điểm cộng dần, không mất đi.

## 15. Quy tắc xác định hệ Pet
### Bước 1: Tính tổng
`Total = Nature + Battle + Build`

### Bước 2: Tính tỉ lệ %
- `Nature% = Nature / Total`
- `Battle% = Battle / Total`
- `Build% = Build / Total`

### Quy tắc tiến hóa (rất quan trọng)
- Nếu 1 hệ ≥ 50% → Pet thuần hệ đó
- Nếu 2 hệ đều ≥ 35% → Pet lai
- Nếu không hệ nào trội → giữ dạng hiện tại

**Ví dụ:**
- Nature 60% → 🌱 Thiên Nhiên
- Nature 40% + Battle 38% → 🌱⚔️ Druid
- Cân đều → chưa tiến hóa

### ⏱️ Khi nào tiến hóa?
Khuyến nghị:
- Mỗi 7 ngày trong game **HOẶC**
- Khi tổng điểm đạt mốc (ví dụ 100)

⛔ **KHÔNG tiến hóa ngay** → tránh “nhảy form liên tục”

## 16. Có nên nuôi nhiều Pet không?
### ✅ Câu trả lời ngắn: CÓ, nhưng KHÔNG NGAY TỪ ĐẦU

### ❌ KHÔNG nên cho nhiều pet ngay vì:
- Scope nổ 💣
- Người chơi không còn gắn bó
- Online cực khó sync
- Game mất “linh hồn”

### ✅ Giải pháp tốt nhất (rất khuyên)
**🐾 Giai đoạn đầu (MVP – 20–30h chơi)**
- Chỉ 1 Linh Thú đồng hành
- Pet = bạn = trọng tâm cảm xúc
- Rất hợp với: cốt truyện + tiến hóa theo lối sống

**🐾 Giai đoạn sau (mid-game)**
- Mở Pet Reserve / Chuồng Linh Thú
- Nuôi 2–3 pet
- Chỉ mang 1 pet ra ngoài
- Pet còn lại:
  - Buff farm
  - Buff build
  - Passive support

**🧬 Giai đoạn late-game**
- Đổi pet để thử lối chơi mới
- Co-op:
  - Mỗi người 1 pet
  - Không “spam pet”

💡 Nếu cho nhiều pet → phải có GIỚI HẠN
Ví dụ:
- Mang theo tối đa 1 pet chiến đấu
- Pet khác chỉ passive
- Đổi pet chỉ ở nhà
