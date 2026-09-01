# Döviz Bilgileri REST API Uygulaması

Bu proje, çeşitli harici döviz servislerinden (Frankfurter, TCMB, Altınkaynak) güncel kur bilgilerini çekerek bu verileri tek bir standart formata dönüştüren ve RESTful API uç noktaları üzerinden sunan bir .NET 10 Web API uygulamasıdır. 

Proje, kurumsal yazılım standartları ve temiz kod (Clean Code) prensipleri gözetilerek geliştirilmiştir.

## 🚀 Kullanılan Teknolojiler & Mimari Yapı

* **Framework:** .NET 10
* **Dil:** C# 
* **Mimari Prensipler:** Katmanlı Mimari (Layered Architecture), SOLID
* **Veri Transferi:** Ortak DTO (Data Transfer Object) Mimarisi
* **Bağımlılık Yönetimi:** Keyed Dependency Injection (İsimlendirilmiş Bağımlılık Enjeksiyonu)
* **API Dökümantasyonu:** Swagger / OpenAPI
* **Versiyon Kontrol:** Git
* **Veri İşleme:** `JsonDocument` (DOM Parsing), `XDocument` (XML Parsing)

## 📌 Mimari Yaklaşım ve Tasarım Kararları

1. **Interface & Abstraction (Soyutlama):** Tüm dış veri kaynakları `ICurrencyService` arayüzünü (interface) uygular. Bu sayede `Controller` katmanı, verinin nereden ve hangi formatta (XML/JSON) geldiğini bilmeden standart bir şekilde çalışır.
2. **Keyed Dependency Injection:** Projede aynı arayüzü uygulayan üç farklı servis (`FrankfurterService`, `TcmbService`, `AltinkaynakService`) bulunmaktadır. Bu servislerin çakışmasını önlemek ve esnekliği artırmak için .NET'in `Keyed Services` altyapısı kullanılmıştır.
3. **Defensive Programming:** Harici servislerden gelebilecek `null`, format uyuşmazlığı veya ondalık ayracı (nokta/virgül) hatalarına karşı merkezi bir `ParseDecimal` kontrol mekanizması yazılmıştır.

## 🔌 API Uç Noktaları (Endpoints)

| HTTP Metodu | Uç Nokta (Endpoint) | Açıklama | Kaynak |
| :--- | :--- | :--- | :--- |
| `GET` | `/api/exchange/frankfurter` | Frankfurter API üzerinden tüm güncel döviz kurlarını getirir. | Frankfurter |
| `GET` | `/api/exchange/tcmb` | Merkez Bankası'nın (XML) güncel döviz kurlarını JSON formatında sunar. | TCMB |
| `GET` | `/api/exchange/altinkaynak` | Altınkaynak güncel kur ve altın fiyatlarını getirir. | Altınkaynak |
| `GET` | `/api/exchange/{code}` | Belirtilen para biriminin (Örn: USD, EUR) kur bilgisini getirir. | Frankfurter |
| `GET` | `/api/exchange/convert?from=USD&to=TRY&amount=100` | Verilen miktar ve kurlar üzerinden çapraz kur (Cross Rate) hesaplaması yapar. | Frankfurter |

## 🛠️ Kurulum ve Çalıştırma

1. Projeyi yerel bilgisayarınıza klonlayın:
   ```bash
   git clone <repository-url>