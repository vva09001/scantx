import React from "react";
import LayoutWrapper from "../../components/utility/layoutWrapper";
import Papersheet from "../../components/utility/papersheet";
import { FullColumn } from "../../components/utility/rowColumn";
import IntlMessages from "../../components/utility/intlMessages";

export default () => (
  <LayoutWrapper>
    <FullColumn>
      <Papersheet title={<IntlMessages id="sidebar.user" />}>
        This is a user page
      </Papersheet>
    </FullColumn>
  </LayoutWrapper>
);
