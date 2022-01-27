# downloadFolderOrganizerWS


İndirilenler klasörü devamlı doluyor ve kısa sürede çarşamba pazarına dönüyor. Bu Windows Service yazılımı ise indirilen dosyaları belirli kategorilere göre düzenliyor.

Program.cs'deki x.RunAs()'deki parametreleri kendi bilgilerinizi girerek kullanabilirsiniz.

herhangi bir hata olursa "Documents" dosyasına log.txt dosyasını oluşturup hatayı içine yazdırıyor.

**Nasıl Yüklenir**

Compile yaptığınız klasörün yolunu kopyalayın. "CMD"'ye "cd <yol>" yazın. Daha sonra "<exe'nin ismi>.exe install start" komutunu girin.

**Silme İşlemi**

Aynı şekilde "CMD"'ye "<exe'nin ismi>.exe uninstall" komutunu yazarak silin.


--  YAPILACAKLAR  --
1. Aynı isimdeki bir dosyayı indirince gerekli klasöre atamıyor.
2. **(Kısmen Bitti)** Hata denetimi artırılacak log dosyasına yazdırılacak.
3. username/password girmeden işlemleri yapılabilecek hale getirilecek.
4. **(Bitti)** İki noktalı isimliler düzeltilecek.
5. **(Bitti)** İndirilen dosya üzerinde işlem yapılamaması.
