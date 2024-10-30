CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    migration_id character varying(150) NOT NULL,
    product_version character varying(32) NOT NULL,
    CONSTRAINT pk___ef_migrations_history PRIMARY KEY (migration_id)
);

START TRANSACTION;

CREATE TABLE desks (
    id uuid NOT NULL DEFAULT (gen_random_uuid()),
    name varchar NOT NULL,
    CONSTRAINT pk_desks PRIMARY KEY (id)
);

CREATE TABLE employee_roles (
    id uuid NOT NULL DEFAULT (gen_random_uuid()),
    name varchar NOT NULL,
    CONSTRAINT pk_employee_roles PRIMARY KEY (id)
);

CREATE TABLE payments (
    id uuid NOT NULL DEFAULT (gen_random_uuid()),
    bill numeric(18,2) NOT NULL,
    tip numeric(18,2),
    created_at timestamp NOT NULL DEFAULT (now()),
    updated_at timestamp NOT NULL DEFAULT (now()),
    CONSTRAINT pk_payments PRIMARY KEY (id)
);

CREATE TABLE product_categories (
    id uuid NOT NULL DEFAULT (gen_random_uuid()),
    name varchar NOT NULL,
    CONSTRAINT pk_product_categories PRIMARY KEY (id)
);

CREATE TABLE users (
    id uuid NOT NULL DEFAULT (gen_random_uuid()),
    name varchar NOT NULL,
    email varchar NOT NULL,
    is_verified boolean NOT NULL DEFAULT FALSE,
    password_hash varchar NOT NULL,
    role varchar NOT NULL,
    CONSTRAINT pk_users PRIMARY KEY (id)
);

CREATE TABLE products (
    id uuid NOT NULL DEFAULT (gen_random_uuid()),
    name varchar NOT NULL,
    description text NOT NULL,
    image_url varchar NOT NULL,
    old_price numeric(18,2),
    price numeric(18,2) NOT NULL,
    category_id uuid NOT NULL,
    CONSTRAINT pk_products PRIMARY KEY (id),
    CONSTRAINT fk_products_product_categories_category_id FOREIGN KEY (category_id) REFERENCES product_categories (id) ON DELETE CASCADE
);

CREATE TABLE customers (
    id uuid NOT NULL DEFAULT (gen_random_uuid()),
    user_id uuid NOT NULL,
    CONSTRAINT pk_customers PRIMARY KEY (id),
    CONSTRAINT fk_customers_users_user_id FOREIGN KEY (user_id) REFERENCES users (id) ON DELETE CASCADE
);

CREATE TABLE employees (
    id uuid NOT NULL DEFAULT (gen_random_uuid()),
    user_id uuid NOT NULL,
    role_id uuid NOT NULL,
    CONSTRAINT pk_employees PRIMARY KEY (id),
    CONSTRAINT fk_employees_employee_roles_role_id FOREIGN KEY (role_id) REFERENCES employee_roles (id) ON DELETE CASCADE,
    CONSTRAINT fk_employees_users_user_id FOREIGN KEY (user_id) REFERENCES users (id) ON DELETE CASCADE
);

CREATE TABLE orders (
    id uuid NOT NULL DEFAULT (gen_random_uuid()),
    customer_id uuid NOT NULL,
    waiter_id uuid NOT NULL,
    desk_id uuid NOT NULL,
    status varchar NOT NULL,
    payment_id uuid,
    created_at timestamp NOT NULL DEFAULT (now()),
    updated_at timestamp NOT NULL DEFAULT (now()),
    CONSTRAINT pk_orders PRIMARY KEY (id),
    CONSTRAINT fk_orders_customers_customer_id FOREIGN KEY (customer_id) REFERENCES customers (id) ON DELETE CASCADE,
    CONSTRAINT fk_orders_desks_desk_id FOREIGN KEY (desk_id) REFERENCES desks (id) ON DELETE CASCADE,
    CONSTRAINT fk_orders_employees_waiter_id FOREIGN KEY (waiter_id) REFERENCES employees (id) ON DELETE CASCADE,
    CONSTRAINT fk_orders_payments_payment_id FOREIGN KEY (payment_id) REFERENCES payments (id)
);

CREATE TABLE order_line_items (
    id uuid NOT NULL DEFAULT (gen_random_uuid()),
    product_id uuid NOT NULL,
    count integer NOT NULL,
    order_id uuid,
    CONSTRAINT pk_order_line_items PRIMARY KEY (id),
    CONSTRAINT fk_order_line_items_orders_order_id FOREIGN KEY (order_id) REFERENCES orders (id),
    CONSTRAINT fk_order_line_items_products_product_id FOREIGN KEY (product_id) REFERENCES products (id) ON DELETE CASCADE
);

INSERT INTO employee_roles (id, name)
VALUES ('4957a460-3fb4-4da3-bd3c-93fea5c243b4', 'waiter');
INSERT INTO employee_roles (id, name)
VALUES ('e665f6f5-2e66-4796-ae9c-d970a6fc0de0', 'manager');

INSERT INTO product_categories (id, name)
VALUES ('06a1e8ac-da51-43e9-ac2c-f5dfcd438e8a', 'Seafood');
INSERT INTO product_categories (id, name)
VALUES ('0fb6a30f-a1f9-4b3e-ac6c-abaa346b5641', 'Drinks');
INSERT INTO product_categories (id, name)
VALUES ('13c1edf0-3ed8-4bfc-b282-5ed19d1f092e', 'Hot Dogs');
INSERT INTO product_categories (id, name)
VALUES ('7345ca63-8cc3-4fc1-9430-473f117d8f96', 'Desserts');
INSERT INTO product_categories (id, name)
VALUES ('9ba99028-8961-4d1c-a5f0-6ce3ae23a487', 'Fast Food');
INSERT INTO product_categories (id, name)
VALUES ('b18349cc-08c7-46f1-a297-c1f94b75628f', 'Coffee');
INSERT INTO product_categories (id, name)
VALUES ('be6b583d-02d0-422f-bb8f-1c4917cda3c1', 'Barbecue');
INSERT INTO product_categories (id, name)
VALUES ('c5d2763e-2822-48fb-a1eb-099676efb89f', 'Sushi');
INSERT INTO product_categories (id, name)
VALUES ('caf63089-65d8-44b5-8a6d-312217a7093a', 'Pizzas');
INSERT INTO product_categories (id, name)
VALUES ('f9a3b34b-019e-4aeb-95ed-f73611fc76a9', 'Steaks');

INSERT INTO users (id, email, name, password_hash, role)
VALUES ('93bfd40b-49a8-459a-b8bd-5b526490e16f', 'victor_samoylov@gmail.com', 'Victor Samoylov', 'D3A9BB4F1ABF2DFD419B038CCFB73165D5D3E542C3DFB962F8DA4550CC44D337-87668C9B71CD59F640FC8B7F34B1D64F', 'employee');

INSERT INTO employees (id, role_id, user_id)
VALUES ('8f93d98a-ed6e-42ac-a5e3-a8c6eff11562', 'e665f6f5-2e66-4796-ae9c-d970a6fc0de0', '93bfd40b-49a8-459a-b8bd-5b526490e16f');

CREATE INDEX ix_customers_user_id ON customers (user_id);

CREATE UNIQUE INDEX ix_desks_name ON desks (name);

CREATE UNIQUE INDEX ix_employee_roles_name ON employee_roles (name);

CREATE INDEX ix_employees_role_id ON employees (role_id);

CREATE INDEX ix_employees_user_id ON employees (user_id);

CREATE INDEX ix_order_line_items_order_id ON order_line_items (order_id);

CREATE INDEX ix_order_line_items_product_id ON order_line_items (product_id);

CREATE INDEX ix_orders_customer_id ON orders (customer_id);

CREATE INDEX ix_orders_desk_id ON orders (desk_id);

CREATE INDEX ix_orders_payment_id ON orders (payment_id);

CREATE INDEX ix_orders_waiter_id ON orders (waiter_id);

CREATE UNIQUE INDEX ix_product_categories_name ON product_categories (name);

CREATE INDEX ix_products_category_id ON products (category_id);

CREATE UNIQUE INDEX ix_products_name ON products (name);

CREATE UNIQUE INDEX ix_users_email ON users (email);

INSERT INTO "__EFMigrationsHistory" (migration_id, product_version)
VALUES ('20241024135542_Initial', '8.0.8');

COMMIT;

