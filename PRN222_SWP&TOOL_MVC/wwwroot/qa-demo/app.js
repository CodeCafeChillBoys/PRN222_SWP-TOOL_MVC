// ====== STATE ======
let currentRole = 'student';

// ====== SCREEN NAVIGATION ======
function showScreen(id) {
  document.querySelectorAll('.screen').forEach(s => s.classList.remove('active'));
  const el = document.getElementById(id);
  if (el) el.classList.add('active');
  // sync sidebar active
  document.querySelectorAll('.sb-item').forEach(i => i.classList.remove('active'));
}

// ====== ROLE TOGGLE ======
function toggleRole() {
  currentRole = currentRole === 'student' ? 'teacher' : 'student';
  const lbl = document.getElementById('roleLabel');
  const btn = document.querySelector('.btn-switch');
  const ss = document.getElementById('sidebarStudent');
  const st = document.getElementById('sidebarTeacher');
  if (currentRole === 'teacher') {
    lbl.textContent = '👨‍🏫 Giáo viên';
    btn.textContent = '🔄 Đổi vai: Học sinh';
    ss.classList.add('hidden');
    st.classList.remove('hidden');
    showScreen('t-inbox');
    showToast('Đã chuyển sang giao diện Giáo viên!');
  } else {
    lbl.textContent = '👨‍🎓 Học sinh';
    btn.textContent = '🔄 Đổi vai: Giáo viên';
    st.classList.add('hidden');
    ss.classList.remove('hidden');
    showScreen('s-dashboard');
    showToast('Đã chuyển sang giao diện Học sinh!');
  }
}

// ====== MODAL ======
function openModal(id) {
  document.getElementById(id).classList.remove('hidden');
  document.body.style.overflow = 'hidden';
}
function closeModal(id) {
  document.getElementById(id).classList.add('hidden');
  document.body.style.overflow = '';
}
// Close modal on overlay click
document.querySelectorAll('.modal-overlay').forEach(o => {
  o.addEventListener('click', e => { if (e.target === o) closeModal(o.id); });
});

// ====== SUBMIT QUESTION ======
function submitQ() {
  closeModal('modalSend');
  showToast('✅ Câu hỏi đã gửi thành công đến TS. Trần Minh Khoa!');
  // fake add row
  const tbody = document.querySelector('.tbl tbody');
  const tr = document.createElement('tr');
  tr.innerHTML = `
    <td>${tbody.children.length + 1}</td>
    <td>Câu hỏi mới vừa gửi...</td>
    <td>${new Date().toLocaleDateString('vi-VN')} ${new Date().toLocaleTimeString('vi-VN', {hour:'2-digit',minute:'2-digit'})}</td>
    <td><span class="badge red">🔴 Chưa trả lời</span></td>
    <td><button class="btn-link" onclick="showScreen('s-thread')">Xem chi tiết →</button></td>`;
  tbody.prepend(tr);
}

// ====== INBOX SELECT ======
function selectInbox(el, nextScreen) {
  document.querySelectorAll('.inbox-item').forEach(i => i.classList.remove('active'));
  el.classList.add('active');
}

// ====== PILL FILTER ======
function setPill(el) {
  document.querySelectorAll('.pill').forEach(p => p.classList.remove('active'));
  el.classList.add('active');
}

// ====== AI ACCEPT ======
function acceptAI() {
  const area = document.getElementById('replyArea');
  area.value = `Để implement JWT trong ASP.NET Core 8, bạn cần:\n\n1. Cài NuGet package: Microsoft.AspNetCore.Authentication.JwtBearer\n\n2. Cấu hình trong Program.cs:\nbuilder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)\n    .AddJwtBearer(options => { ... });\n\n3. Tạo JwtService để generate & validate token.\n\n4. Thêm [Authorize] attribute lên controller cần bảo vệ.\n\nNếu cần thêm ví dụ cụ thể, em hỏi thêm nhé!`;
  showToast('🤖 Đã nhập gợi ý AI vào ô soạn thảo!');
}

function editAI() {
  acceptAI();
  document.getElementById('replyArea').focus();
}

// ====== SEND REPLY ======
function sendReply(mode) {
  const content = document.getElementById('replyArea').value.trim();
  if (!content) { showToast('⚠️ Vui lòng nhập nội dung câu trả lời!', 'warn'); return; }
  const saveKB = document.getElementById('saveKB').checked;
  if (mode === 'public') {
    showToast('📢 Đã đăng công khai lên FAQ! Tất cả nhóm có thể xem.');
  } else {
    showToast('💬 Đã gửi trả lời trực tiếp đến Nhóm 3!');
  }
  if (saveKB) setTimeout(() => showToast('🧠 Đã lưu vào Knowledge Base của AI!'), 1500);
  setTimeout(() => showScreen('t-inbox'), 2000);
}

// ====== FAQ ======
function toggleFAQ(el) {
  const ans = el.querySelector('.faq-a');
  const arrow = el.querySelector('.faq-arrow');
  const isOpen = ans.classList.contains('open');
  // close all
  document.querySelectorAll('.faq-a').forEach(a => a.classList.remove('open'));
  document.querySelectorAll('.faq-arrow').forEach(a => a.textContent = '▶');
  if (!isOpen) {
    ans.classList.add('open');
    arrow.textContent = '▼';
  }
}

function filterFAQ(val) {
  const q = val.toLowerCase();
  document.querySelectorAll('.faq-item').forEach(item => {
    const text = item.querySelector('.faq-q span').textContent.toLowerCase();
    item.style.display = text.includes(q) ? '' : 'none';
  });
}

function setCat(el) {
  document.querySelectorAll('.cat').forEach(c => c.classList.remove('active'));
  el.classList.add('active');
}

function likeIt(btn) {
  btn.textContent = '✅ Đã đánh dấu';
  btn.disabled = true;
  const likeEl = btn.parentElement;
  const txt = likeEl.firstChild;
  const cur = parseInt(txt.textContent.match(/\d+/)[0]) + 1;
  txt.textContent = `👍 ${cur} nhóm thấy hữu ích `;
}

// ====== TOAST ======
function showToast(msg, type='ok') {
  const t = document.createElement('div');
  t.className = 'toast';
  if (type === 'warn') t.style.borderColor = 'var(--orange)';
  t.textContent = msg;
  document.body.appendChild(t);
  setTimeout(() => t.remove(), 3000);
}

// ====== KEYBOARD ======
document.addEventListener('keydown', e => {
  if (e.key === 'Escape') {
    document.querySelectorAll('.modal-overlay:not(.hidden)').forEach(m => closeModal(m.id));
  }
});

// Init
showScreen('s-dashboard');
