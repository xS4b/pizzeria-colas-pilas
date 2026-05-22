# 🍕 Práctica Evaluada: Pizzería Campus Express
**Gestión de Pedidos con Colas y Pilas en C# / WinForms**

## 🎯 Objetivo
Implementar un sistema de gestión de pedidos que respete el orden de llegada (FIFO) y permita revertir la última acción del operador (LIFO), utilizando colecciones de .NET y entregando el trabajo mediante un flujo profesional de Git.

## 📖 Escenario
Trabajas en el sistema de una pizzería universitaria:
- **🚲 Cola de Pedidos (`Queue`)**: Los clientes llaman y sus pedidos se registran. Por norma de la casa, **se preparan y entregan estrictamente por orden de llegada**.
- **📦 Pila de Bitácora (`Stack`)**: Cada acción del operador se registra. Si hay un error, el botón **"Deshacer"** revierte **la última acción realizada**.

## ⚙️ Requerimientos Técnicos
| Componente | Especificación |
|------------|----------------|
| **Colecciones** | `System.Collections.Generic` (`Queue<string>`, `Stack<string>`) |
| **Formato de texto** | Usar exclusivamente `string.Format()`. |
| **Interfaz** | WinForms con `txtCliente`, `lstPedidos`, `lstBitacora`, `lblEstado`, `lblContador` y botones de acción |
| **Entrega** | Fork → Commits ≥3 → Pull Request al repositorio base |

## 🛠️ Guía de Implementación (Paso a Paso)
> 📌 Sigue este orden lógico. Los comentarios en `Form1.cs` te marcarán exactamente dónde escribir cada bloque.

1. **Nuevo Pedido (Entrada FIFO)**
   - Lee y limpia el texto del cliente.
   - Si está vacío, muestra advertencia en `lblEstado` usando `string.Format()` y detén la ejecución.
   - Agrega el cliente a la cola.
   - Registra la acción en la pila con prefijo `"PEDIDO: "`.
   - Actualiza la interfaz.

2. **Preparar/Entregar (Salida FIFO)**
   - Valida que haya pedidos pendientes. Si no, informa y retorna.
   - Extrae el primer cliente de la cola.
   - Registra `"ENTREGADO: {cliente}"` en la pila.
   - Refresca la UI y muestra confirmación.

3. **Deshacer Última Acción (Lógica LIFO)**
   - Si la bitácora está vacía, informa y retorna.
   - Extrae la última acción registrada.
   - Analiza el prefijo:
     - Si fue `"PEDIDO:"` → El cliente **nunca salió** de la cola. Debes reconstruir la cola excluyendo ese registro (pista: usa `ToArray()`, limpia la cola original y vuelve a `Enqueue()` solo los elementos que no coincidan).
     - Si fue `"ENTREGADO:"` → El cliente **ya fue despachado**. Debes volver a encolarlo extrayendo su nombre del texto.
   - Muestra qué se revirtió y sincroniza la interfaz.

4. **Sincronizar Interfaz**
   - Limpia ambos `ListBox`.
   - Itera la cola y la pila, agregando cada elemento a su respectivo `ListBox`.
   - Actualiza `lblContador` con `string.Format("Pedidos: {0} | Bitácora: {1}", cola.Count, pila.Count)`.
   - Si la cola está vacía, agrega un placeholder visual: `"(Sin pedidos pendientes)"`.

## 🌐 Flujo Git para Entrega
1. Haz **Fork** de este repositorio.
2. Clona este fork y abre el `.sln` en SharpDevelop.
3. Implementa la lógica siguiendo los `TODO` de `MainForm.cs`.
4. Realiza **mínimo 3 commits** descriptivos (`feat:`, `fix:`, `docs:`).
5. Haz `git push` y abre un **Pull Request** a este repositorio.
6. En la descripción del PR, responde las **4 Preguntas de Comprensión** y adjunta una captura de la app funcionando.

## ❓ Preguntas de Comprensión (Obligatorias en el PR)
1. ¿Por qué un sistema de delivery usa `Queue` para los pedidos pero `Stack` para la bitácora? ¿Qué problema surgiría si invertimos las estructuras?

El procesamiento de pedidos sigue una estructura Queue (FIFO) para garantizar la justicia en la atención, entregando la comida en el estricto orden de llegada de los clientes. Por el contrario, un registro de actividad o bitácora se gestiona como Stack (LIFO) porque el interés técnico está en auditar el incidente más reciente, el cual se posiciona arriba de todo para un acceso inmediato.
   
2. ¿Por qué es obligatorio verificar `Count == 0` antes de `Dequeue()` o `Pop()`? ¿Qué ocurre en ejecución si se omite?

Esta validación es el escudo de seguridad que impide realizar operaciones de extracción en estructuras vacías. Omitir este chequeo provoca un error de desbordamiento o ejecución inválida (InvalidOperationException) que interrumpe el flujo del sistema de golpe, provocando el colapso y cierre de la aplicación.
 
3. En el método `Deshacer`, ¿por qué es necesario analizar el texto con `.StartsWith()` antes de revertir? ¿Qué error lógico evitaría esto?

La función de este método es inspeccionar el inicio de la cadena de texto almacenada para identificar la naturaleza de la acción previa. Gracias a esto, el sistema no actúa a ciegas y puede ejecutar con precisión la lógica de reversión adecuada, transformando una inserción en una baja, o viceversa.

4. ¿Qué ventaja tiene entregar mediante Fork + Pull Request en lugar de un archivo comprimido? ¿Cómo facilita la la retroalimentación?

La metodología de Fork y Pull Request optimiza la entrega de tareas al filtrar automáticamente carpetas temporales de compilación (bin/obj) que ensucian los archivos .zip. Esta dinámica agiliza la corrección en la nube mediante anotaciones directas del docente y permite aplicar parches en el mismo historial mediante un simple push, sin duplicar entregas.



## ✅ Checklist de Entrega
- [ ] Código compila en SharpDevelop sin warnings críticos
- [ ] Uso correcto y consistente de `string.Format()` en todos los mensajes
- [ ] Botón `Deshacer` revierte correctamente ambos casos
- [ ] ≥3 commits con mensajes claros
- [ ] PR abierto con respuestas + captura

📥 **Entrega:** Pull Request. Fecha límite: fin de la sesión de laboratorio.
