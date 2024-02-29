# Modulitis

Modulitis jest zestawem narzędzi do tworzenia aplikacji modularnej, w której każdy moduł jest oddzielnym procesem. Dodatkowo pozwala na zarządzanie modułami w czasie działania programu.

## Problem

W architekturze monolitycznej, wszystkie komponenty systemu są ze sobą ściśle powiązane i działają jako jedna jednostka. W przypadku wystąpienia błędu krytycznego, może to prowadzić do awarii całego systemu, co jest poważnym problemem. Wykrycie i naprawa takiego błędu muszą nastąpić natychmiast, co może być trudne i czasochłonne.

Z drugiej strony, w architekturze mikroserwisów, każda usługa działa niezależnie. Oznacza to, że awaria jednej usługi nie wpływa na działanie innych. To sugeruje, że mikroserwisy mogą być bardziej odporne na błędy i łatwiejsze do zarządzania. Jednakże, taka architektura wiąże się z większą złożonością, zarówno pod względem implementacji, jak i zarządzania.

W ramach niniejszego repozytorium oferujemy rozwiązanie, które łączy zalety obu tych architektur. Mimo zastosowania architektury monolitycznej, umożliwia ono izolację awarii dzięki zastosowaniu wydzielonych modułów i procesów.

Kluczowe korzyści z tego rozwiązania to:
1. **Izolacja awarii**: Każdy moduł działa niezależnie, więc awaria jednego modułu nie wpływa na działanie innych.
2. **Zarządzanie w czasie rzeczywistym**: Moduły mogą być monitorowane i zarządzane w czasie rzeczywistym, co umożliwia szybką reakcję na problemy.
3. **Skalowalność**: Możliwość uruchomienia dodatkowych instancji wydzielonych modułów lub wielu instalacji procesów dla jednego modułu umożliwia skalowanie systemu w zależności od potrzeb.
4. **Monitoring**: Każdy moduł może być monitorowany indywidualnie, co umożliwia precyzyjne śledzenie wydajności i identyfikację problemów.
5. **Śledzenie zapytań**: Możliwość śledzenia przebiegu zapytań pomiędzy modułami umożliwia lepsze zrozumienie interakcji w systemie i identyfikację potencjalnych punktów zapalnych.
6. **Odseparowanie kodu**: Każdy moduł ma swój własny, odseparowany kod, co ułatwia zarządzanie i utrzymanie kodu.

Dzięki temu rozwiązaniu, możliwe jest skuteczne zarządzanie błędami, a także optymalizacja i skalowanie systemu, bez konieczności rezygnacji z zalet płynących z architektury monolitycznej. Jest to innowacyjne podejście, które łączy zalety obu architektur, oferując jednocześnie nowe możliwości.

## Uwagi

Docelowo przetłumaczyć na angielski

