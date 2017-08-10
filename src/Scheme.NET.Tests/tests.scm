(test (eqv? #t #t) #t)
(test (eqv? #f #f) #t)
(test (eqv? #t #f) #f)

(test (eqv? 'test 'test) #t)
(test (eqv? 'tEst 'test) #t)
(test (eqv? 'test 'foo) #f)

(test (eqv? 1 1) #t)
(test (eqv? 1.0 1.0) #t)
(test (eqv? 1/2 1/2) #t)
(test (eqv? 1+1i 1+1i) #t)
(test (eqv? 1.0+1.0i 1.0+1.0i) #t)
(test (eqv? 1/2+1/2i 1/2+1/2i) #t)

(test (eqv? 1.0 1) #f)
(test (eqv? 0.5 1/2) #f)
(test (eqv? 1.0+1.0i 1+1i) #f)
(test (eqv? 0.5+0.5i 1/2+1/2i) #f)

(test (eqv? 1 2) #f)
(test (eqv? 1.0 2.0) #f)
(test (eqv? 1/2 2/2) #f)
(test (eqv? 1+1i 2+1i) #f)
(test (eqv? 1+1i 1+2i) #f)
(test (eqv? 1.0+1.0i 2.0+1.0i) #f)
(test (eqv? 1.0+1.0i 1.0+2.0i) #f)
(test (eqv? 1/2+1/2i 2/2+1/2i) #f)
(test (eqv? 1/2+1/2i 1/2+2/2i) #f)

(test (eqv? #\a #\a) #t)
(test (eqv? #\a #\z) #f)
(test (eqv? #\z #\z) #t)

(test (eqv? () ()) #t)


; need define for these!
; (define a (1 . 2))
; (define a2 (1 . 2))
; (define b #(1 2))
; (define b2 #(1 2))
; (define c "test")
; (define c2 "test")

; (test (eqv? a a) #t)
; (test (eqv? a a2) #f)
; (test (eqv? b b) #t)
; (test (eqv? b b2) #f)
; (test (eqv? c c) #t)
; (test (eqv? c c2) #f)

(test (eqv? + +) #t)
(test (eqv? + -) #f)

; I'm super lazy so and its still in spec for 
; these to actually just be the same procedure
; so this test ensures that when I eventually change
; it I will remember to properly write tests for it
(test (eqv? eqv? eq?) #t)

(test (equal? #t #t) #t)
(test (equal? #f #f) #t)
(test (equal? #t #f) #f)

(test (equal? 'test 'test) #t)
(test (equal? 'tEst 'test) #t)
(test (equal? 'test 'foo) #f)

(test (equal? 1 1) #t)
(test (equal? 1.0 1.0) #t)
(test (equal? 1/2 1/2) #t)
(test (equal? 1+1i 1+1i) #t)
(test (equal? 1.0+1.0i 1.0+1.0i) #t)
(test (equal? 1/2+1/2i 1/2+1/2i) #t)

(test (equal? 1.0 1) #f)
(test (equal? 0.5 1/2) #f)
(test (equal? 1.0+1.0i 1+1i) #f)
(test (equal? 0.5+0.5i 1/2+1/2i) #f)

(test (equal? 1 2) #f)
(test (equal? 1.0 2.0) #f)
(test (equal? 1/2 2/2) #f)
(test (equal? 1+1i 2+1i) #f)
(test (equal? 1+1i 1+2i) #f)
(test (equal? 1.0+1.0i 2.0+1.0i) #f)
(test (equal? 1.0+1.0i 1.0+2.0i) #f)
(test (equal? 1/2+1/2i 2/2+1/2i) #f)
(test (equal? 1/2+1/2i 1/2+2/2i) #f)

(test (equal? #\a #\a) #t)
(test (equal? #\a #\z) #f)
(test (equal? #\z #\z) #t)

(test (equal? () ()) #t)


; need define for these!
; (define a (1 . 2))
; (define a2 (1 . 2))
; (define b #(1 2))
; (define b2 #(1 2))
; (define c "test")
; (define c2 "test")

; (test (equal? a a) #t)
; (test (equal? a a2) #f)
; (test (equal? b b) #t)
; (test (equal? b b2) #f)
; (test (equal? c c) #t)
; (test (equal? c c2) #f)

(test (equal? + +) #t)
(test (equal? + -) #f)