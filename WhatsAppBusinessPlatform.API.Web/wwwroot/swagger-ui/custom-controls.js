// wwwroot/swagger-ui/custom-controls.js
(function () {
    const LANG_KEY = 'swagger-lang';
    const TOKEN_KEY = 'swagger-token';
    const DEFAULT_LANG = 'en-US';
    const ALLOWED_LANGS = [
        { value: 'en-US', label: 'English (en-US)' },
        { value: 'ar-EG', label: 'Arabic (ar-EG)' },
        { value: 'fr-FR', label: 'French (fr-FR)' },
        { value: 'es-ES', label: 'Spanish (es-ES)' }
    ];

    const ACCENT = '#2ecc71';
    const ACCENT_DARK = '#20b66a';
    const RADIUS = '4px';
    const PILL_CLASS = 'custom-swagger-pill';

    // Create pill DOM (idempotent factory)
    function createPill(type) {
        const wrapper = document.createElement('div');
        wrapper.classList.add(PILL_CLASS, `custom-swagger-pill-${type}`);
        wrapper.style.display = 'inline-flex';
        wrapper.style.alignItems = 'center';
        wrapper.style.padding = '6px 10px';
        wrapper.style.borderRadius = RADIUS;
        wrapper.style.border = `2px solid ${ACCENT}`;
        wrapper.style.background = '#ffffff';
        wrapper.style.boxShadow = `0 2px 6px rgba(32,182,106,0.12)`;
        wrapper.style.fontWeight = '600';
        wrapper.style.gap = '8px';
        wrapper.style.cursor = 'pointer';
        wrapper.style.userSelect = 'none';
        wrapper.style.marginRight = '8px';
        wrapper.style.height = '33.2px';
        wrapper.style.boxSizing = 'border-box';
        wrapper.addEventListener('mouseenter', () => {
            wrapper.style.boxShadow = `0 4px 10px rgba(32,182,106,0.18)`;
            wrapper.style.borderColor = ACCENT_DARK;
        });
        wrapper.addEventListener('mouseleave', () => {
            wrapper.style.boxShadow = `0 2px 6px rgba(32,182,106,0.12)`;
            wrapper.style.borderColor = ACCENT;
        });

        if (type === 'lang') {
            wrapper.setAttribute('title', 'Select Accept-Language');

            const icon = document.createElement('span');
            icon.innerHTML = `
              <svg width="18" height="18" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg" aria-hidden="true" focusable="false">
                <text x="1.5" y="15.2"
                      font-family="Segoe UI, Roboto, 'Noto Sans', Arial, sans-serif"
                      font-size="12.5"
                      font-weight="600"
                      fill="${ACCENT}"
                      style="line-height:1;">
                  文
                </text>
                <text x="11.2" y="16.2"
                      font-family="Segoe UI, Roboto, Arial, sans-serif"
                      font-size="14"
                      font-weight="700"
                      fill="${ACCENT}"
                      style="line-height:1;">
                  A
                </text>
              </svg>
            `;
            icon.style.display = 'inline-flex';

            const text = document.createElement('span');
            text.textContent = 'Lang';
            text.style.color = ACCENT;
            text.style.fontSize = '13px';

            const select = document.createElement('select');
            select.id = 'swLang';
            select.style.border = 'none';
            select.style.outline = 'none';
            select.style.background = 'transparent';
            select.style.fontWeight = '700';
            select.style.color = ACCENT;
            select.style.fontSize = '13px';
            select.style.minWidth = '120px';
            select.style.cursor = 'pointer';
            select.style.appearance = 'none';
            select.style.paddingRight = '18px';
            select.style.height = '25px';

            ALLOWED_LANGS.forEach(l => {
                const opt = document.createElement('option');
                opt.value = l.value;
                opt.text = l.label;
                select.appendChild(opt);
            });

            select.value = localStorage.getItem(LANG_KEY) || DEFAULT_LANG;
            select.addEventListener('change', () => {
                localStorage.setItem(LANG_KEY, select.value);
            });

            const caret = document.createElement('span');
            caret.innerHTML = `<svg width="12" height="12" viewBox="0 0 24 24" fill="${ACCENT}" xmlns="http://www.w3.org/2000/svg"><path d="M7 10l5 5 5-5H7z"/></svg>`;
            caret.style.display = 'inline-flex';

            wrapper.appendChild(icon);
            wrapper.appendChild(text);
            wrapper.appendChild(select);
            wrapper.appendChild(caret);

            // expose for debug
            wrapper._select = select;
        }

        if (type === 'login') {
            wrapper.style.borderColor = '#49cc90';
            wrapper.style.boxShadow = '0 2px 6px rgba(73,204,144,0.12)';

            const icon = document.createElement('span');
            icon.innerHTML = `<svg width="18" height="18" viewBox="0 0 24 24" fill="#49cc90" xmlns="http://www.w3.org/2000/svg" aria-hidden="true"><path d="M12 12c2.7 0 4.9-2.2 4.9-4.9S14.7 2.2 12 2.2 7.1 4.4 7.1 7.1 9.3 12 12 12zm0 2.4c-3.2 0-9.6 1.6-9.6 4.8v2.4h19.2v-2.4c0-3.2-6.4-4.8-9.6-4.8z"/></svg>`;
            icon.style.display = 'inline-flex';

            const text = document.createElement('span');
            text.textContent = 'Login';
            text.style.color = '#49cc90';
            text.style.fontSize = '13px';

            wrapper.appendChild(icon);
            wrapper.appendChild(text);

            wrapper.addEventListener('click', showLoginModal);
        }

        return wrapper;
    }

    // Login modal (kept simple)
    function showLoginModal() {
        if (document.getElementById('swagger-login-modal')) return;
        const overlay = document.createElement('div');
        overlay.id = 'swagger-login-modal';
        overlay.style.position = 'fixed';
        overlay.style.top = 0;
        overlay.style.left = 0;
        overlay.style.width = '100%';
        overlay.style.height = '100%';
        overlay.style.background = 'rgba(0,0,0,0.5)';
        overlay.style.display = 'flex';
        overlay.style.alignItems = 'center';
        overlay.style.justifyContent = 'center';
        overlay.style.zIndex = 9999;

        const modal = document.createElement('div');
        modal.style.background = '#fff';
        modal.style.borderRadius = '8px';
        modal.style.padding = '20px';
        modal.style.minWidth = '320px';
        modal.style.boxShadow = '0 6px 20px rgba(0,0,0,0.3)';
        modal.innerHTML = `
      <h3 style="margin-top:0; color:#49cc90;">Login</h3>
      <div style="margin-bottom:10px;">
        <input id="swagger-username" type="text" value="batman@gmail.com" placeholder="Username" style="width:100%; padding:8px; box-sizing:border-box;" />
      </div>
      <div style="margin-bottom:10px;">
        <input id="swagger-password" type="password" value="Aa@12345678" placeholder="Password" style="width:100%; padding:8px; box-sizing:border-box;" />
      </div>
      <div style="text-align:right;">
        <button id="swagger-login-cancel" style="margin-right:8px; background:#eee; border:none; padding:8px 12px; border-radius:6px; cursor:pointer;">Cancel</button>
        <button id="swagger-login-submit" style="background:#49cc90; border:none; color:#fff; padding:8px 14px; border-radius:6px; cursor:pointer; font-weight:600;">Login</button>
      </div>
    `;
        overlay.appendChild(modal);
        document.body.appendChild(overlay);
        document.getElementById('swagger-login-cancel').onclick = () => overlay.remove();
        document.getElementById('swagger-login-submit').onclick = async () => {
            const email = document.getElementById('swagger-username').value;
            const password = document.getElementById('swagger-password').value;
            if (!email || !password) {
                alert('Please fill email and password');
                return;
            }
            try {
                const res = await fetch('/Auths/Login', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({ email, password })
                });
                if (!res.ok) {
                    const txt = await res.text().catch(() => null);
                    throw new Error(txt || 'Login failed');
                }
                const data = await res.json();
                sessionStorage.setItem(TOKEN_KEY, data.token);
                alert('Login successful!');
                overlay.remove();
            } catch (err) {
                alert('Login failed: ' + (err.message || err));
            }
        };
    }

    // Build and keep single instances
    function getOrCreateControls() {
        let lang = document.querySelector(`.${PILL_CLASS}.custom-swagger-pill-lang`);
        let login = document.querySelector(`.${PILL_CLASS}.custom-swagger-pill-login`);
        if (!lang) lang = createPill('lang');
        if (!login) login = createPill('login');
        // ensure they have small spacing
        lang.style.marginRight = '6px';
        login.style.marginRight = '8px';
        // return as nodes (ensure they are not duplicated in DOM)
        return { lang, login };
    }

    // Find preferred insertion point: schemes section then auth-wrapper's parent
    function findSchemesSection() {
        const selectors = [
            'section.schemes.wrapper.block.col-12',
            'section.schemes.wrapper.block',
            '.schemes.wrapper',
            '.schemes'
        ];
        for (const s of selectors) {
            const el = document.querySelector(s);
            if (el) return el;
        }
        return null;
    }
    function findAuthorizeContainerFallback() {
        const selectors = [
            '.auth-wrapper',
            '.swagger-ui .topbar .auth-wrapper',
            '.swagger-ui .topbar .download-url-wrapper + *',
            '.swagger-ui .topbar .wrapper',
            '.swagger-ui .topbar'
        ];
        for (const s of selectors) {
            const el = document.querySelector(s);
            if (el) return el;
        }
        return null;
    }

    // Remove duplicate pills (only keep first set)
    function cleanupDuplicates() {
        const all = Array.from(document.querySelectorAll(`.${PILL_CLASS}`));
        if (all.length <= 2) return; // expected: 2 (lang + login)
        // keep first occurrences of lang/login, remove rest
        const seen = new Set();
        for (const n of all) {
            const key = n.className;
            if (seen.has(key)) n.remove();
            else seen.add(key);
        }
    }

    // Positioning logic (idempotent)
    function ensurePillsPosition() {
        try {
            const { lang, login } = getOrCreateControls();
            cleanupDuplicates();

            // If they're already correctly placed (same parent and before auth), do nothing
            const schemesSection = findSchemesSection();
            if (schemesSection) {
                const authWrapper = schemesSection.querySelector('.auth-wrapper');
                if (authWrapper && authWrapper.parentNode) {
                    // Insert before authWrapper
                    if (lang.parentNode !== authWrapper.parentNode || login.parentNode !== authWrapper.parentNode || lang.nextSibling === authWrapper || login.nextSibling === authWrapper) {
                        authWrapper.parentNode.insertBefore(login, authWrapper);
                        authWrapper.parentNode.insertBefore(lang, login);
                    }
                    return;
                } else {
                    // no auth-wrapper inside schemes: put pills at start of the section
                    if (lang.parentNode !== schemesSection) schemesSection.insertBefore(lang, schemesSection.firstChild);
                    if (login.parentNode !== schemesSection) schemesSection.insertBefore(login, lang.nextSibling);
                    return;
                }
            }

            // fallback: try existing authorize container
            const authContainer = findAuthorizeContainerFallback();
            if (authContainer && authContainer.parentNode) {
                if (lang.parentNode !== authContainer.parentNode) authContainer.parentNode.insertBefore(lang, authContainer);
                if (login.parentNode !== authContainer.parentNode) authContainer.parentNode.insertBefore(login, authContainer);
                return;
            }

            // last resort: attach to topbar or body
            const topbar = document.querySelector('.swagger-ui .topbar') || document.querySelector('.swagger-ui');
            if (topbar) {
                // create a wrapper only if not already present
                let wrapper = topbar.querySelector('.custom-swagger-topbar-wrapper');
                if (!wrapper) {
                    wrapper = document.createElement('div');
                    wrapper.classList.add('custom-swagger-topbar-wrapper');
                    wrapper.style.display = 'inline-flex';
                    wrapper.style.alignItems = 'center';
                    wrapper.style.marginLeft = 'auto';
                    topbar.appendChild(wrapper);
                }
                if (lang.parentNode !== wrapper) wrapper.appendChild(lang);
                if (login.parentNode !== wrapper) wrapper.appendChild(login);
                return;
            }

            // absolute fallback
            if (lang.parentNode !== document.body) document.body.appendChild(lang);
            if (login.parentNode !== document.body) document.body.appendChild(login);
        } catch (err) {
            // swallow errors to avoid breaking swagger UI
            console.warn('ensurePillsPosition error', err);
        }
    }

    // Initial attempt; Swagger UI may not be ready yet so we'll also observe
    ensurePillsPosition();

    // Observe DOM mutations to re-position pills if Swagger re-renders the schemes/auth part
    const observer = new MutationObserver((mutations) => {
        let useful = false;

        for (const m of mutations) {
            // check added nodes
            if (m.addedNodes && m.addedNodes.length) {
                for (const n of m.addedNodes) {
                    if (n.nodeType !== 1) continue; // skip non-elements

                    // prefer classList (safe), fallback to getAttribute('class') string
                    try {
                        if (n.classList) {
                            if (n.classList.contains('schemes') || n.classList.contains('auth-wrapper') || n.classList.contains('topbar')) {
                                useful = true;
                                break;
                            }
                        }
                    } catch (e) {
                        // ignore classList errors and try attribute fallback below
                    }

                    const attr = (typeof n.getAttribute === 'function') ? (n.getAttribute('class') || '') : '';
                    if (attr.indexOf('schemes') !== -1 || attr.indexOf('auth-wrapper') !== -1 || attr.indexOf('topbar') !== -1) {
                        useful = true;
                        break;
                    }
                }
            }
            if (useful) break;

            // check removed nodes
            if (m.removedNodes && m.removedNodes.length) {
                for (const n of m.removedNodes) {
                    if (n.nodeType !== 1) continue;

                    try {
                        if (n.classList) {
                            if (n.classList.contains('schemes') || n.classList.contains('auth-wrapper') || n.classList.contains('topbar')) {
                                useful = true;
                                break;
                            }
                        }
                    } catch (e) { }

                    const attr = (typeof n.getAttribute === 'function') ? (n.getAttribute('class') || '') : '';
                    if (attr.indexOf('schemes') !== -1 || attr.indexOf('auth-wrapper') !== -1 || attr.indexOf('topbar') !== -1) {
                        useful = true;
                        break;
                    }
                }
            }
            if (useful) break;
        }

        if (useful) {
            if (observer._timer) clearTimeout(observer._timer);
            observer._timer = setTimeout(() => {
                ensurePillsPosition();
            }, 60);
        }
    });

    observer.observe(document.documentElement || document.body, { childList: true, subtree: true });

    // also run a few times in case MutationObserver misses first render
    let tries = 0;
    const t = setInterval(() => {
        ensurePillsPosition();
        tries++;
        if (tries > 8) clearInterval(t);
    }, 300);

    // ---- patch fetch to attach Accept-Language & Authorization (keeps your current behavior) ----
    (function () {
        const originalFetch = window.fetch.bind(window);
        window.fetch = function (input, init) {
            const lang = localStorage.getItem(LANG_KEY) || DEFAULT_LANG;
            const token = sessionStorage.getItem(TOKEN_KEY);

            let headersInstance;
            if (init && init.headers) {
                if (init.headers instanceof Headers) {
                    headersInstance = init.headers;
                } else if (Array.isArray(init.headers)) {
                    headersInstance = new Headers(init.headers);
                } else {
                    headersInstance = new Headers();
                    Object.keys(init.headers).forEach(k => headersInstance.set(k, init.headers[k]));
                }
            } else if (input instanceof Request) {
                headersInstance = new Headers(input.headers);
            } else {
                headersInstance = new Headers();
            }

            if (lang) headersInstance.set('Accept-Language', lang);
            if (token) headersInstance.set('Authorization', 'Bearer ' + token);

            if (input instanceof Request) {
                const newReq = new Request(input, { headers: headersInstance });
                return originalFetch(newReq);
            } else {
                init = init || {};
                init.headers = headersInstance;
                return originalFetch(input, init);
            }
        };
    })();

})();
